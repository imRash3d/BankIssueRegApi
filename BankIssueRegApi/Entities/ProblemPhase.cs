using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Entities
{
    public class ProblemPhase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string ProblemLeadName { get; set; }
        public string ProblemLeadEmail { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int Claim { get; set; }
        public string Tags { get; set; }
        public string Agents { get; set; }
        public int Insurance { get; set; }
        public bool IsApproved { get; set; } = false;
        public string Files { get; set; }
     //   public ICollection<Stakeholder> Stakeholders { get; set; }
        public int ProblemId { get; set; }
        public string BusinessImpact { get; set; }
    }
}
