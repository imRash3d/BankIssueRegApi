using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Entities
{
    public class EmailTemplate
    {
        public int Id { get; set; }
        public string TemplateBody { get; set; }
        public string TemplateName { get; set; }
        public string TemplateSubject { get; set; }

    }
}
