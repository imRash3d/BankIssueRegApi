using AutoMapper;
using BankIssueRegApi.Dtos;
using BankIssueRegApi.Infrastructure.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BankIssueRegApi.Infrastructure.Services
{
    public class UploadService : IUploadService
    {

        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;


        public UploadService(IWebHostEnvironment environment, IMapper mapper)
        {
            _environment = environment;
            _mapper = mapper;

        }
        public FileDto UploadFile(IFormFile file)
        {

          

        var folderName = Path.Combine(_environment.ContentRootPath,"Uploads");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
              
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return new FileDto {FileName=fileName, FilePath=dbPath };
            }
            else
            {
                return null;
            }
        }
    }
}
