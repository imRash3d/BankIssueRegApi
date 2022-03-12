using BankIssueRegApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Dtos
{
    public class StackholderDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public Department Department { get; set; }

        public int PhaseId { get; set; }
    }
}
