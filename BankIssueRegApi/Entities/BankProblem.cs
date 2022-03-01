using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Entities
{
    public class BankProblem
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Department { get; set; }
        public List<string> Tags { get; set; }
        public List<string> DepartmentCode { get; set; }
        public string Comments { get; set; }
        public string ProblemLeadName { get; set; }
        public string ProblemLeadEmail { get; set; }
        public string Site { get; set; }
        public string ExternalLink { get; set; }
        public bool IsAnlysisRequired { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Issue Claim { get; set; }
        public Issue Insurance { get; set; }
        // public string FileUrl { get; set; }


    }
}
