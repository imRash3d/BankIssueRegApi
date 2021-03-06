using BankIssueRegApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Dtos
{
    public class BankProblemDto
    {
     
        public string Title { get; set; }
        public string Text { get; set; }
        public Department Department { get; set; }
      
        public List<string> Tags { get; set; }
        public List<string> DepartmentCode { get; set; }
        public string Comments { get; set; }
        public string ProblemLeadName { get; set; }
        public string ProblemLeadEmail { get; set; }
        public string Site { get; set; }
        public string ExternalLink { get; set; }
        public bool IsAnlysisRequired { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public IssueDto Claim { get; set; }
        public IssueDto Insurance { get; set; }
        public int Id { get; set; }
        public List<Agent> Agents { get; set; }
        public DateTime ToWhen { get; set; }
        public DateTime FromWhen { get; set; }
        public bool IsApproved { get; set; }
        //  public string FileName { get; set; }  // not used anymore
        public List<FileDto> Files { get; set; }
    }
}
