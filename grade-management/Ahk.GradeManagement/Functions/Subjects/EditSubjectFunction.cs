using System.Net;
using System.Threading.Tasks;

using Ahk.GradeManagement.Data.Entities;
using Ahk.GradeManagement.Services.SubjectService;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Ahk.GradeManagement.Functions.Subjects
{
    public class EditSubjectFunction
    {
        private readonly ILogger _logger;
        private readonly ISubjectService subjectService;

        public EditSubjectFunction(ILoggerFactory loggerFactory, ISubjectService subjectService)
        {
            _logger = loggerFactory.CreateLogger<EditSubjectFunction>();
            this.subjectService = subjectService;
        }

        [Function("edit-subject")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "edit-subject")] HttpRequestData req,
            [FromBody] SubjectDTO subjectDTO)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var subject = new Subject
            {
                Id = subjectDTO.Id,
                Name = subjectDTO.Name,
                Semester = subjectDTO.Semester,
                SubjectCode = subjectDTO.SubjectCode,
                GithubOrg = subjectDTO.GithubOrg,
                AhkConfig = subjectDTO.AhkConfig,
            };

            await subjectService.EditSubjectAsync(subject);

            return new OkResult();
        }
    }
}
