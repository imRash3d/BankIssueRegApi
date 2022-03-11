using AutoMapper;
using BankIssueRegApi.Dtos;
using BankIssueRegApi.Entities;
using BankIssueRegApi.Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Infrastructure.Services
{
    public class BankProblemService : IBankProblemService
    {
        private readonly DbContextService _context;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;


        public BankProblemService(DbContextService context, IMapper mapper, IMailService mailService)
        {
            _context = context;
            _mapper = mapper;
            _mailService = mailService;

        }
        public async Task<bool> AddProblem(CreateProblemDto model)

        {

         BankProblem bankProblemModel = CreateProblemModel(model);
        
         _context.BankProblems.Add(bankProblemModel);
          SendMailToProblemLead(bankProblemModel);
          return await SaveAllAsync();
        }



        private void SendMailToProblemLead(BankProblem model)
        {
            var dateContext = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            dateContext.Add("IssueName", model.Title);

            EmailModelDto emailModel = new EmailModelDto
            {
                EmailTemplateName = "ApprovalConfirm",
                To = model.ProblemLeadEmail,
                DataContext = dateContext
            };

            _mailService.SendMail(emailModel);
        }


        private BankProblem CreateProblemModel(CreateProblemDto model)
        {
            // create claim & insureance 

            var ClaimModel = CreateIssueModel(model.Claim);
           Issue claim = ClaimModel != null ? CreateIssue(ClaimModel) : null;

              var insuranceModel = CreateIssueModel(model.Insurance);
            Issue insurance = insuranceModel != null ? CreateIssue(insuranceModel) : null;



            BankProblem bankProblemModel = new BankProblem
            {
                Title = model.Title,
                Comments = model.Comments,
                Text = model.Text,
                Site = model.Site,
                DepartmentId = model.Department.Id,
                ExternalLink = model.ExternalLink,
                FileName = model.FileName,
                ProblemLeadName = model.ProblemLeadName,
                ProblemLeadEmail = model.ProblemLeadEmail,
                DepartmentCode = JsonConvert.SerializeObject(model.DepartmentCode),
                Tags = JsonConvert.SerializeObject(model.Tags),

                Agents = JsonConvert.SerializeObject(model.Agents.Select(x => x.Id).ToList()),
                Claim = claim != null ? claim.Id : 0,
                Insurance = insurance != null ? insurance.Id : 0,
                ToWhen = model.ToWhen,
                FromWhen = model.FromWhen,
                IsAnlysisRequired = model.IsAnlysisRequired

            };

            return bankProblemModel;
        }

        private Issue CreateIssueModel(IssueDto model)
        {
            if (model == null) return null;
            return new Issue
            {
                Category = JsonConvert.SerializeObject(model.Category),
                Code = model.Code,
                Family = model.Family,
                FamilyDivision = model.FamilyDivision
            };
        }


        private dynamic CreateIssue(Issue model)
        {
           
            _context.Issues.Add(model);
            _context.SaveChanges();
            return model;
        }

        public async Task<bool> DeleteProblem(int  id)
        {
            var problem = _context.BankProblems.SingleOrDefault(x => x.Id == id);
            var claim = _context.Issues.SingleOrDefault(x => x.Id == problem.Claim);
            var insurance = _context.Issues.SingleOrDefault(x => x.Id == problem.Claim);
            if (problem != null){
                _context.BankProblems.Remove(problem);
            }

            if (claim != null)
            {
                _context.Issues.Remove(claim);
            }

            if (insurance != null)
            {
                _context.Issues.Remove(insurance);
            }

            return await SaveAllAsync();

         
        }

        public BankProblemDto GetProblem(int id)
        {
            var result = new BankProblemDto();
            var bankProblem = _context.BankProblems.SingleOrDefault(x => x.Id == id);
            if (bankProblem != null)
            {
                List<BankProblemDto> results = GetProblemsDto(new List<BankProblem> { bankProblem });
                result = results.Find(x => x.Id == id);
                return result;
            }
            else
            {
                return null;
            }
            
            
        }

        public async Task<List<BankProblemDto>> GetProblems()
        {
            var bankProblems = await _context.BankProblems.ToListAsync();


            List<BankProblemDto> results = GetProblemsDto(bankProblems);
            return results;
        }



        private List<BankProblemDto> GetProblemsDto(List<BankProblem> problems)
        {
            List<BankProblemDto> results = new List<BankProblemDto>();

            foreach (var problem in problems)
            {
                var problemDto = _mapper.Map<BankProblemDto>(problem);
                var department = _context.Departments.SingleOrDefault(x => x.Id == problem.DepartmentId);
                problemDto.Department = department;
                var ids = JsonConvert.DeserializeObject(problem.Agents);
                problemDto.Claim = CreateIssueDto(problem.Claim);
                problemDto.Insurance = CreateIssueDto(problem.Insurance);
                problemDto.Agents = getAgents(ids);
                results.Add(problemDto);
            }

            results.Sort((a, b) => b.CreatedDate.CompareTo(a.CreatedDate));
            return results;
        }



        private IssueDto CreateIssueDto(int id)
        {
            
           var result = _context.Issues.SingleOrDefault(x => x.Id == id);
           var issue = _mapper.Map<IssueDto>(result);
            return result != null ? issue : null;

        }

        private List<Agent> getAgents(dynamic ids )
        {
            List<Agent> agents = new List<Agent> { };
            foreach (int id in ids)
            {
              var agent=  _context.Agents.SingleOrDefault(x => x.Id == id);
                agents.Add(agent);
            }
            return agents;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void UpdateProblem(CreateProblemDto model)
        {
            // BankProblem bankProblemModel = CreateProblemModel(model);

            var problem = new BankProblem
            {
                Id = model.Id,
                Title = model.Title,
                Comments = model.Comments,
                Text = model.Text,
                Site = model.Site,
                DepartmentId = model.Department.Id,
                ExternalLink = model.ExternalLink,
                ProblemLeadName = model.ProblemLeadName,
                ProblemLeadEmail = model.ProblemLeadEmail,
                DepartmentCode = JsonConvert.SerializeObject(model.DepartmentCode),
                Tags = JsonConvert.SerializeObject(model.Tags),

                Agents = JsonConvert.SerializeObject(model.Agents.Select(x => x.Id).ToList()),
                 Claim = model.Claim!=null ? model.Claim.Id : 0,
                 Insurance = model.Insurance != null ? model.Insurance.Id : 0,
                ToWhen = model.ToWhen,
                FromWhen = model.FromWhen,
                IsAnlysisRequired = model.IsAnlysisRequired
            };

            if (model.Claim != null)
            {
                UpdateIssue(model.Claim);
            }
            if (model.Insurance != null)
            {
                UpdateIssue(model.Insurance);
            }

            _context.BankProblems.Update(problem);
        }


        private void UpdateIssue(IssueDto model)
        {
            var issue = new Issue
            {
                Category = JsonConvert.SerializeObject(model.Category),
                Code = model.Code,
                Family = model.Family,
                FamilyDivision = model.FamilyDivision,
                Id = model.Id
            };
            _context.Issues.Update(issue);
        }

        public async Task<bool> ApprovedProblem(ApprovedProblemDto model)
        {
            var bankProblem = _context.BankProblems.SingleOrDefault(x => x.Id == model.Id);
             bankProblem.IsApproved = model.IsApproved;
            _context.BankProblems.Update(bankProblem);
            return await SaveAllAsync();
        }
    }
}
