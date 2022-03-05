using BankIssueRegApi.Dtos;
using BankIssueRegApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Infrastructure.Contracts
{
   public interface IBankProblemService
    {
        void AddProblem(CreateProblemDto model);
        Task<bool> SaveAllAsync();
        void UpdateProblem(dynamic model);
        Task<bool> DeleteProblem(int id);
        BankProblem GetProblem(int productId);
        Task<List<BankProblemDto>> GetProblems();
    }
}
