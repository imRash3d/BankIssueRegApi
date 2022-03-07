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
        Task<bool> AddProblem(CreateProblemDto model);
        Task<bool> SaveAllAsync();
        void UpdateProblem(CreateProblemDto model);
        Task<bool> DeleteProblem(int id);
        BankProblemDto GetProblem(int productId);
        Task<List<BankProblemDto>> GetProblems();

        Task<bool> ApprovedProblem(ApprovedProblemDto model);
    }
}
