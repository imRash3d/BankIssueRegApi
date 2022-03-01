using BankIssueRegApi.Entities;
using BankIssueRegApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Controllers
{
    public class BankProblemController : ApiController
    {
        private readonly ILogger<BankProblemController> _logger;
        public BankProblemController(ILogger<BankProblemController> logger)
        {
            _logger = logger;
        }


    

        [HttpGet("list")]
        public async Task<ActionResult<CommandResponse>> SendMessages()

        {

            CommandResponse response = new CommandResponse();
            
            var issues = new List<BankProblem>();
            issues.Add(new BankProblem
            {
                Title = "Issue1",
                Comments = " this is simple comments",
                Text = "Bank Issue Problem 1 ",
                Site="Site 01",
                Department = "Local",
                IsAnlysisRequired = false,
                ExternalLink="",
                CreatedDate  = DateTime.Now,
                DepartmentCode = new List<string> { "001 Front Office" },
                Tags = new List<string> { "Claim" },
                ProblemLeadEmail = "sample@yopmail.com",
                ProblemLeadName = "Jhon Doe",
                Claim = new Issue
                {
                    Code = "001",
                    Family = "Local",
                    FamilyDivision = "",
                    Category = new List<string> { "Sample Category" },
                },
                Insurance = null


            }
            
            );
            issues.Add(new BankProblem
            {
                Title = "Issue2",
                Comments = " this is simple comments",
                Text = "Bank Issue Problem 2 ",
                Site = "Site 01",
                Department = "Local",
                IsAnlysisRequired = false,
                ExternalLink = "",
                CreatedDate = DateTime.Now,
                DepartmentCode = new List<string> { "001 Front Office" },
                Tags = new List<string> { "Claim" },
                ProblemLeadEmail = "sample@yopmail.com",
                ProblemLeadName = "Jhon Doe",
                Insurance = new Issue
                {
                    Code = "001",
                    Family = "Local",
                    FamilyDivision = "",
                    Category = new List<string> { "Sample Category" },
                },
                Claim=null


            }

            );
            issues.Add(new BankProblem
            {
                Title = "Issue1",
                Comments = " this is simple comments",
                Text = "Bank Issue Problem 1 ",
                Site = "Site 01",
                Department = "Local",
                IsAnlysisRequired = false,
                ExternalLink = "",
                CreatedDate = DateTime.Now,
                DepartmentCode = new List<string> { "001 Front Office" },
                Tags = new List<string> { "Claim" },
                ProblemLeadEmail = "sample@yopmail.com",
                ProblemLeadName = "Jhon Doe",
                Claim = new Issue
                {
                    Code = "001",
                    Family = "Local",
                    FamilyDivision = "",
                    Category = new List<string> { "Sample Category" },
                },
                Insurance = null


            }
            );
       
            response.Result = issues;
            response.Success = true;
            return await Task.FromResult(response);
        }
    }
}
