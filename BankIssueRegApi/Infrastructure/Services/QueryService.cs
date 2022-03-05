using AutoMapper;
using BankIssueRegApi.Entities;
using BankIssueRegApi.Infrastructure.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Infrastructure.Services
{
    public class QueryService:IQueryService
    {
        private readonly DbContextService _context;
        private readonly IMapper _mapper;


        public QueryService(DbContextService context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public Agent GetAgent(int id)
        {
            return _context.Agents.SingleOrDefault(x => x.Id == id);
        }

        public List<Agent> GetAgents()
        {
            return _context.Agents.ToList();
        }

        public Department GetDepartment(int id)
        {
            return _context.Departments.SingleOrDefault(x => x.Id == id);
        }

        public List<Department> GetDepartments()
        {
            return _context.Departments.ToList();
        }
    }
}
