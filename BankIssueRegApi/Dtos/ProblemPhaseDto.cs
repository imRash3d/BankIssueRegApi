using BankIssueRegApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Dtos
{
    public class ProblemPhaseDto
    {
     
        public string Title { get; set; }
        public Department Department { get; set; }
      
        public List<string> Tags { get; set; }
        public List<string> DepartmentCode { get; set; }
        public string ProblemLeadName { get; set; }
        public string ProblemLeadEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public IssueDto Claim { get; set; }
        public IssueDto Insurance { get; set; }
        public int Id { get; set; }
        public List<Agent> Agents { get; set; }
        public bool IsApproved { get; set; }
        public List<FileDto> Files { get; set; }
        public int ProblemId { get; set; }
        public string BusinessImpact { get; set; }
        public ICollection<Stakeholder> Stakeholders { get; set; }
    }
}
