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
        


        public BankProblemService(DbContextService context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public void AddProblem(CreateProblemDto model)

        {

         BankProblem bankProblemModel = CreateProblemModel(model);
         _context.BankProblems.Add(bankProblemModel);
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

        public void DeleteProblem(BankProblem model)
        {
            _context.Remove(model);
        }

        public BankProblem GetProblem(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BankProblemDto>> GetProblems()
        {
            var bankProblems = await _context.BankProblems.ToListAsync();
          

            List<BankProblemDto> results = new List<BankProblemDto>();

            foreach (var problem in bankProblems)
            {
                var problemDto = _mapper.Map<BankProblemDto>(problem);
                var department = _context.Departments.SingleOrDefault(x => x.Id == problem.DepartmentId);
                problemDto.Department = department;
                var ids  = JsonConvert.DeserializeObject(problem.Agents);
                problemDto.Claim = CreateIssueDto(problem.Claim);
                problemDto.Insurance = CreateIssueDto(problem.Insurance);
                problemDto.Agents = getAgents(ids);
                results.Add(problemDto);
            }
           
          
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

        public void UpdateProblem(dynamic model)
        {
            BankProblem bankProblemModel = CreateProblemModel(model);
            _context.BankProblems.Update(model);
        }
    }
}
