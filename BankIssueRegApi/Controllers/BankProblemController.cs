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


        // get problem 
        [HttpGet("{id}")]
        public async Task<ActionResult<CommandResponse>> GetProblem(int id)

        {

            CommandResponse response = new CommandResponse();

            try
            {
                var problem =  _bankProblemService.GetProblem(id);

                response.Result = problem;
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

          var result= await _bankProblemService.AddProblem(model);
            if (result)
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


        [HttpPost("add-phase")]
        public async Task<ActionResult<CommandResponse>> addProblemPhase(CreateProblePhasemDto model)
        {
            CommandResponse response = new CommandResponse();

            var result = await _bankProblemService.AddProblemPhase(model);
            if (result)
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


        [HttpPost("update/{id}")]
        public async Task<ActionResult<CommandResponse>> updateProblem(CreateProblemDto model)
        {
            CommandResponse response = new CommandResponse();

            _bankProblemService.UpdateProblem(model);
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

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<CommandResponse>> deleteProblem(int id)
        {
            CommandResponse response = new CommandResponse();

           var isDeleted=  await _bankProblemService.DeleteProblem(id);
            if (isDeleted)
            {
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.ErrorMessage = "Failed to delete problem";
                return BadRequest(response);
            }

            return await Task.FromResult(response);
        }



        [HttpPost("approved-problem")]
        public async Task<ActionResult<CommandResponse>> ApprovedProblem(ApprovedProblemDto model)
        {
            CommandResponse response = new CommandResponse();

            var isDeleted = await _bankProblemService.ApprovedProblem(model);
            if (isDeleted)
            {
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.ErrorMessage = "Failed to approved problem";
                return BadRequest(response);
            }

            return await Task.FromResult(response);
        }



        // get phase 
        [HttpGet("phase/{id}")] //problem id
        public async Task<ActionResult<CommandResponse>> GetPhase(int id)

        {

            CommandResponse response = new CommandResponse();

            try
            {
                var phase = _bankProblemService.GetPhase(id);

                response.Result = phase;
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



        [HttpPost("update-phase/{id}")]
        public async Task<ActionResult<CommandResponse>> updatePhase(CreateProblePhasemDto model)
        {
            CommandResponse response = new CommandResponse();

          
            if (await _bankProblemService.UpdateProblemPhase(model))
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
