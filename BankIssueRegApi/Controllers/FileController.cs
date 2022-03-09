using BankIssueRegApi.Helpers;
using BankIssueRegApi.Infrastructure.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BankIssueRegApi.Controllers
{
    public class FileController : ApiController
    {

        private readonly IUploadService _uploadService;
        private readonly ILogger<FileController> _logger;
        public FileController(
            IUploadService uploadService,
            ILogger<FileController> logger)
        {
            _logger = logger;
            _uploadService = uploadService;
        }




        [HttpPost("upload-file")]
        public async Task<ActionResult<CommandResponse>> UploadProductImage(IFormFile file)
        {
            CommandResponse response = new CommandResponse();
            var result =  _uploadService.UploadFile(file);

            if (result== null)
            {
                response.ErrorMessage = "Failed to uplaod File";
                return BadRequest(response);
            }

          

            response.Result = result;
            response.Success = true;
            return await Task.FromResult(response);
        }

        [HttpGet("{filename}")]
        public async Task<IActionResult> Download(string fileName)
        {
            CommandResponse response = new CommandResponse();
            if (fileName == null)
            {
                response.ErrorMessage = "Failed to uplaod File";
                return BadRequest(response);
            }
            //Build the File Path.


            string path = _uploadService.FilePath(fileName);


            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);

           
        }


    

    }
}
