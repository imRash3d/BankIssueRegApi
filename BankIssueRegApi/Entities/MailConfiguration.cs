using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Entities
{
    public class MailConfiguration
    {
        public int id { get; set; }
        public string MailConfigurationId { get; set; }
        public string Host { get; set; }
        public string MailAccountPassword { get; set; }
        public string MailSenderAddress { get; set; }
        public string MailSenderUserName { get; set; }
        public int Port { get; set; }
    }
}
