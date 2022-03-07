using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Entities
{
    public class BankProblem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string Comments { get; set; }
        public string ProblemLeadName { get; set; }
        public string ProblemLeadEmail { get; set; }
        public string Site { get; set; }
        public string ExternalLink { get; set; }
        public bool IsAnlysisRequired { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int Claim { get; set; }
        public string Tags { get; set; }
        public string Agents { get; set; }
        public int Insurance { get; set; }
        public DateTime ToWhen { get; set; }
        public DateTime FromWhen { get; set; }
        // public string FileUrl { get; set; }
        public bool IsApproved { get; set; } = false;
        public string FileName { get; set; }


    }
}
