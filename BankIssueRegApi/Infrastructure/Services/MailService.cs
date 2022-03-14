using BankIssueRegApi.Dtos;
using BankIssueRegApi.Entities;
using BankIssueRegApi.Extensions;
using BankIssueRegApi.Infrastructure.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankIssueRegApi.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly DbContextService _context;
        private readonly IConfiguration _configuration;
        // private readonly ILogger _logger;
        public MailService(DbContextService context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            // _logger = logger;
        }
        public void SendMail(EmailModelDto emailDataDto)
        {
            MailConfiguration mailConfiguration = _context.MailConfigurations.SingleOrDefault(x => x.MailConfigurationId == _configuration["MailConfigurationId"]);

           // EmailTemplate emailTemplate = _context.EmailTemplates.SingleOrDefault(x => x.TemplateName == emailDataDto.EmailTemplateName);


            if (emailDataDto != null && mailConfiguration != null)
            {
                string to = emailDataDto.To; //To address    
                string from = mailConfiguration.MailSenderAddress; //From address    
                MailMessage message = new MailMessage(from, to);
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                };

                string templateBody = System.IO.File.ReadAllText("./EmailTemplates/approval.html");
                string mailbody = Regex.Unescape(templateBody);      // unescape  Mailtemaplate 



                // formate Mailtemaplate with place holder 

                Regex re = new Regex(@"\{(\w+)\}", RegexOptions.Compiled);
                emailDataDto.DataContext["Link"] = _configuration["ApprovedUrl"] + emailDataDto.DataContext["IssueId"];
                string outputMailBody = re.Format(mailbody, emailDataDto.DataContext);



                message.Subject = "Approval Email";

                message.Body = outputMailBody;
                //message.Body = "<html><body><div>You have been invited to approve "+emailDataDto.DataContext["IssueName"] +" this problem</div></body></html>";
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient(mailConfiguration.Host, mailConfiguration.Port); //Gmail smtp    
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential(mailConfiguration.MailSenderUserName, mailConfiguration.MailAccountPassword);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                try
                {
                    client.Send(message);
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // _logger.LogError(ex.Message);
                }
            }

        }
    }
}

