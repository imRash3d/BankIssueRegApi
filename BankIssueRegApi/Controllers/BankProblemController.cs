using BankIssueRegApi.Dtos;
using BankIssueRegApi.Entities;
using BankIssueRegApi.Helpers;
using BankIssueRegApi.Infrastructure.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Controllers
{
    public class BankProblemController : ApiController
    {
        private readonly IBankProblemService _bankProblemService;
        private readonly ILogger<BankProblemController> _logger;
        public BankProblemController(
            IBankProblemService bankProblemService,
            ILogger<BankProblemController> logger)
        {
            _logger = logger;
            _bankProblemService = bankProblemService;
        }

     


        [HttpGet("list")]
        public async Task<ActionResult<CommandResponse>> GetProblems()

        {

            CommandResponse response = new CommandResponse();

            try {
                var problems = await _bankProblemService.GetProblems();

                response.Result = problems;
                response.Success = true;
            }
            catch (Exception ex)
            {


                response.Result = null;
                response.Success = false;
                response.ErrorMessage = JsonConvert.SerializeObject(ex);
            }
            return await Task.FromResult(response);
        }


        
        [HttpPost("add")]
        public async Task<ActionResult<CommandResponse>> addProblem(CreateProblemDto model)
        {
            CommandResponse response = new CommandResponse();

            _bankProblemService.AddProblem(model);
            if (await _bankProblemService.SaveAllAsync())
            {
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.ErrorMessage = "Failed to add problem";
                return BadRequest(response);
            }

            return await Task.FromResult(response);
        }

    

    }
}
