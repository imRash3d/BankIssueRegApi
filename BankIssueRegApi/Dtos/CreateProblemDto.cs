﻿using BankIssueRegApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Dtos
{
    [Keyless]
    public class CreateProblemDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public Department Department { get; set; }
        [Required]
      
        public List<string> DepartmentCode { get; set; }
       // [Required]
        public string Comments { get; set; }
        [Required]
        public string ProblemLeadName { get; set; }
        [Required]
        public string ProblemLeadEmail { get; set; }
        [Required]
        public string Site { get; set; }
        [Required]
        public string ExternalLink { get; set; }
        [Required]
        public bool IsAnlysisRequired { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public IssueDto Claim { get; set; }
        public IssueDto Insurance { get; set; }
        public int Id { get; set; }
        [Required]
        public List<string> Tags { get; set; }
        [Required]
        public List<Agent> Agents { get; set; }
        [Required]
        public DateTime ToWhen { get; set; }
        [Required]
        public DateTime FromWhen { get; set; }
        [Required]
        public string FileName { get; set; }
    }
}
