using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Dtos
{
    public class EmailModelDto
    {
        public string EmailTemplateName { get; set; }
        public string To { get; set; }
        public Dictionary<string, string> DataContext { get; set; }
    }
}
