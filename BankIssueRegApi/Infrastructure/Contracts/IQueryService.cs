using BankIssueRegApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Infrastructure.Contracts
{
   public interface IQueryService
    {
        List<Department> GetDepartments();
        List<Agent> GetAgents();
        Agent GetAgent(int id);
        Department GetDepartment(int id);
    }
}
