using AutoMapper;
using BankIssueRegApi.Dtos;
using BankIssueRegApi.Entities;
using BankIssueRegApi.Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            await SaveAllAsync();
            SendMailToProblemLead(bankProblemModel);
            return true;
        }





        private void SendMailToProblemLead(BankProblem model)
        {
            var dateContext = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            dateContext.Add("Title", model.Title);
            dateContext.Add("DisplayName", model.ProblemLeadName);
            dateContext.Add("Tags", String.Join(",", model.Tags));
            dateContext.Add("DepartmentCode", model.DepartmentCode);
            dateContext.Add("FromWhen", model.FromWhen.ToString("MM/dd/yyyy"));
            dateContext.Add("FromTo", model.ToWhen.ToString("MM/dd/yyyy"));
            dateContext.Add("IssueId", model.Id+""); 

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
                Files= JsonConvert.SerializeObject(model.Files),
                Agents = JsonConvert.SerializeObject(model.Agents.Select(x => x.Id).ToList()),
                Claim = claim != null ? claim.Id : 0,
                Insurance = insurance != null ? insurance.Id : 0,
                ToWhen = model.ToWhen,
                FromWhen = model.FromWhen,
                IsAnlysisRequired = model.IsAnlysisRequired

            };

            return bankProblemModel;
        }


        private ProblemPhase CreateProblemPhaseModel(CreateProblePhasemDto model)
        {
            // create claim & insureance 

            var ClaimModel = CreateIssueModel(model.Claim);
            Issue claim = ClaimModel != null ? CreateIssue(ClaimModel) : null;

            var insuranceModel = CreateIssueModel(model.Insurance);
            Issue insurance = insuranceModel != null ? CreateIssue(insuranceModel) : null;



            ProblemPhase bankProblemModel = new ProblemPhase
            {
                Title = model.Title,
                ProblemId= model.ProblemId,
                DepartmentId = model.Department.Id,
                ProblemLeadName = model.ProblemLeadName,
                ProblemLeadEmail = model.ProblemLeadEmail,
                DepartmentCode = JsonConvert.SerializeObject(model.DepartmentCode),
                Tags = JsonConvert.SerializeObject(model.Tags),
                Files = JsonConvert.SerializeObject(model.Files),
                Agents = JsonConvert.SerializeObject(model.Agents.Select(x => x.Id).ToList()),
                Claim = claim != null ? claim.Id : 0,
                Insurance = insurance != null ? insurance.Id : 0,
                BusinessImpact = model.BusinessImpact,
              
                
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
                Files = JsonConvert.SerializeObject(model.Files),
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

        public async Task<bool> AddProblemPhase(CreateProblePhasemDto model)
        {

             ProblemPhase problemPhaseModel = CreateProblemPhaseModel(model);

              _context.ProblemPhases.Add(problemPhaseModel);
               await SaveAllAsync();
               AddStakeholders(model, problemPhaseModel.Id);
               return await SaveAllAsync();
        }


        private void AddStakeholders(CreateProblePhasemDto model, int phaseId)
        {
            foreach (var stakeholder in model.Stakeholders)
            {
                var stakeholderModel = new Stakeholder
                {
                    Department = stakeholder.Department,
                    Name = stakeholder.Name,
                    Email = stakeholder.Email,
                    PhaseId = phaseId
                };

                _context.Stakeholders.Add(stakeholderModel);


                //  SendMailToProblemLead(bankProblemModel);

            };

           
        }

        public ProblemPhaseDto GetPhase(int problemId)
        {
            var result = new ProblemPhaseDto();
            var phase = _context.ProblemPhases.SingleOrDefault(x => x.ProblemId == problemId);
            if (phase != null)
            {
                result = _mapper.Map<ProblemPhaseDto>(phase);
                var department = _context.Departments.SingleOrDefault(x => x.Id == phase.DepartmentId);
                result.Department = department;
                var ids = JsonConvert.DeserializeObject(phase.Agents);
                result.Claim = CreateIssueDto(phase.Claim);
                result.Insurance = CreateIssueDto(phase.Insurance);
                result.Agents = getAgents(ids);
                result.Stakeholders = _context.Stakeholders.Where(x => x.PhaseId == phase.Id).ToList();
                return result;
            }
            else
            {
                return null;
            }

        }

        public async Task<bool> UpdateProblemPhase(CreateProblePhasemDto model)
        {
            // BankProblem bankProblemModel = CreateProblemModel(model);

            var phase = new ProblemPhase
            {
                ProblemId= model.ProblemId,
                DepartmentId = model.Department.Id,
                Id = model.Id,
                Title = model.Title,
                BusinessImpact = model.BusinessImpact,
                ProblemLeadName = model.ProblemLeadName,
                ProblemLeadEmail = model.ProblemLeadEmail,
                DepartmentCode = JsonConvert.SerializeObject(model.DepartmentCode),
                Tags = JsonConvert.SerializeObject(model.Tags),
                Files = JsonConvert.SerializeObject(model.Files),
                Agents = JsonConvert.SerializeObject(model.Agents.Select(x => x.Id).ToList()),
                Claim = model.Claim != null ? model.Claim.Id : 0,
                Insurance = model.Insurance != null ? model.Insurance.Id : 0,
            };

            if (model.Claim != null)
            {
                UpdateIssue(model.Claim);
            }
            if (model.Insurance != null)
            {
                UpdateIssue(model.Insurance);
            }

            if (model.Stakeholders != null)
            {
                UpdateStakeholders(model.Stakeholders, phase.Id,phase);
            }

            if (model.DeleteStakeholderIds != null && model.DeleteStakeholderIds.Count>0)
            {
                DeleteStakeholders(model);
            }

            _context.ProblemPhases.Update(phase);
            return await SaveAllAsync();
        }

        private void DeleteStakeholders(CreateProblePhasemDto model)
        {
            foreach(var id in model.DeleteStakeholderIds)
            {
                var stakeholder = _context.Stakeholders.SingleOrDefault(x => x.Id == id);
                if (stakeholder != null)
                {
                    _context.Stakeholders.Remove(stakeholder);
                }
            }
          
        }


        private void UpdateStakeholders(ICollection<Stakeholder> stakeholders, int phaseId, ProblemPhase phase)
        {
            foreach (var stakeholder in stakeholders)
            {
                if (stakeholder.Id>0)
                {
                    var updateModel = new Stakeholder
                    {
                        Department = stakeholder.Department,
                        Name = stakeholder.Name,
                        Email = stakeholder.Email,
                        PhaseId = phaseId,
                        Id= stakeholder.Id
                    };
                    _context.Stakeholders.Update(updateModel);

                }else
                {
                    var stakeholderModel = new Stakeholder
                    {
                        Department = stakeholder.Department,
                        Name = stakeholder.Name,
                        Email = stakeholder.Email,
                        PhaseId = phaseId
                    };

                    _context.Stakeholders.Add(stakeholderModel);
                    SendMailStakeholderModel(phase, stakeholder.Email);
                }



       

            };


        }


        private void SendMailStakeholderModel(ProblemPhase model, string personEmail)
        {
           
            var dateContext = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            dateContext.Add("Title", model.Title);
            dateContext.Add("DisplayName", model.ProblemLeadName);
            dateContext.Add("Tags", String.Join(",", model.Tags));
            dateContext.Add("DepartmentCode", model.DepartmentCode);
            dateContext.Add("FromWhen", "-");
            dateContext.Add("FromTo", "-");
            dateContext.Add("IssueId", model.ProblemId + "");

            EmailModelDto emailModel = new EmailModelDto
            {
                EmailTemplateName = "ApprovalConfirm",
                To = personEmail,
                DataContext = dateContext
            };

            _mailService.SendMail(emailModel);
        }

        public void TestMail()
        {
            var dateContext = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            dateContext.Add("Title", "test");
            dateContext.Add("DisplayName", "Rishi");
            dateContext.Add("Tags", "Claim");
            dateContext.Add("DepartmentCode", "001");
            dateContext.Add("FromWhen", "12");
            var dt = DateTime.Parse("2022-03-19T06:00:00");
            dateContext.Add("FromTo", dt.ToString("MM/dd/yyyy"));
            dateContext.Add("IssueId", "12"); // test id 
            EmailModelDto emailModel = new EmailModelDto
            {
                EmailTemplateName = "ApprovalConfirm",
                To = "sampleuser05@yopmail.com",
                DataContext = dateContext
            };

            _mailService.SendMail(emailModel);
        }
    }
}
