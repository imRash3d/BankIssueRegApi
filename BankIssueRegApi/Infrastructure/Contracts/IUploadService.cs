using BankIssueRegApi.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Infrastructure.Contracts
{
    public interface IUploadService
    {
        FileDto UploadFile(IFormFile file);
        string FilePath(string fileName);
    }
}
