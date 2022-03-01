using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Entities
{
    public class Issue
    {
        public string Code { get; set; }
        public List<string> Category { get; set; }
        public string Family { get; set; }
        public string FamilyDivision { get; set; }

    }
}
