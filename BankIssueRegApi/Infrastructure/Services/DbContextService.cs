using BankIssueRegApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Infrastructure.Services
{
    public class DbContextService : DbContext
    {
        public DbSet<BankProblem> BankProblems { get; set; }
        public DbSet<ProblemPhase> ProblemPhases { get; set; }
        public DbSet<MailConfiguration> MailConfigurations { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<Department> Departments { get; set; }
        // public DbSet<UploadFile> Files { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Stakeholder> Stakeholders { get; set; }

        public DbContextService(DbContextOptions<DbContextService> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API to configure  

            // Map entities to tables  
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BankProblem>().ToTable("problems");
            modelBuilder.Entity<ProblemPhase>().ToTable("problem_phases");
            modelBuilder.Entity<Stakeholder>().ToTable("stakeholders");
            modelBuilder.Entity<Department>().ToTable("departments");
            modelBuilder.Entity<Agent>().ToTable("agents");
            // modelBuilder.Entity<UploadFile>().ToTable("files");
            modelBuilder.Entity<Issue>().ToTable("problem_related_to");
            modelBuilder.Entity<MailConfiguration>().ToTable("mailconfigurations");
            modelBuilder.Entity<EmailTemplate>().ToTable("email_templates");
         
        }
    }
}
