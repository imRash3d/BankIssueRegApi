using AutoMapper;
using BankIssueRegApi.Dtos;
using BankIssueRegApi.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<BankProblem, BankProblemDto>() 

                .ForMember(y => y.Tags, option => option.MapFrom(x => JsonConvert.DeserializeObject(x.Tags)))
                .ForMember(y => y.DepartmentCode, option => option.MapFrom(x => JsonConvert.DeserializeObject(x.DepartmentCode)))
                .ForMember(y => y.Claim, option => option.Ignore())
                .ForMember(y => y.Insurance, option => option.Ignore())
                .ForMember(y => y.Agents, option => option.MapFrom(x => JsonConvert.DeserializeObject(x.Agents)))
                .ForMember(y => y.Files, option => option.MapFrom(x => JsonConvert.DeserializeObject(x.Files)));

            CreateMap<Issue, IssueDto>()
                 .ForMember(y => y.Category, option => option.MapFrom(x => JsonConvert.DeserializeObject(x.Category)));


        }

        private IssueDto returnIssue(string _issue)
        {
            var issue = (IssueDto)JsonConvert.DeserializeObject(_issue);
            
         
            Console.WriteLine(issue);
            return issue;
        }
    }
}
