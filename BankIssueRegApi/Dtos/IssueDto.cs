using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Dtos
{
    public class IssueDto
    {

        public int Id { get; set; }
        public string Code { get; set; }
     
        public string[] Category { get; set; }
       
        public string Family { get; set; }
      
        public string FamilyDivision { get; set; }
    }
}
