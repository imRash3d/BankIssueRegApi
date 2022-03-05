using BankIssueRegApi.Helpers;
using BankIssueRegApi.Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Controllers
{
    public class QueryController : ApiController
    {
        private readonly IQueryService _queryService;
        private readonly ILogger<QueryController> _logger;
        public QueryController(
            IQueryService queryService,
            ILogger<QueryController> logger)
        {
            _logger = logger;
            _queryService = queryService;
        }



        [HttpGet("departments")]
        public async Task<ActionResult<CommandResponse>> GetDepartments()
        {

            CommandResponse response = new CommandResponse();
            var departments = _queryService.GetDepartments();
            response.Result = departments;
            response.Success = true;
            return await Task.FromResult(response);
        }

        [HttpGet("agents")]
        public async Task<ActionResult<CommandResponse>> GetAgents()
        {

            CommandResponse response = new CommandResponse();
            var agents = _queryService.GetAgents();
            response.Result = agents;
            response.Success = true;
            return await Task.FromResult(response);
        }

    }
}
