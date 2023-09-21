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
    public class CreateSubjectFunction
    {
        private readonly ILogger _logger;
        private readonly ISubjectService subjectService;

        public CreateSubjectFunction(ILoggerFactory loggerFactory, ISubjectService subjectService)
        {
            _logger = loggerFactory.CreateLogger<CreateSubjectFunction>();
            this.subjectService = subjectService;
        }

        [Function("create-subject")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "create-subject")] HttpRequestData req,
            [FromBody] SubjectDTO subjectDTO)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var subject = new Subject
            {
                Name = subjectDTO.Name,
                SubjectCode = subjectDTO.SubjectCode,
                Semester = subjectDTO.Semester,
                GithubOrg = subjectDTO.GithubOrg,
                AhkConfig = subjectDTO.AhkConfig,
            };

            await subjectService.CreateSubjectAsync(subject);

            return new OkResult();
        }
    }
}
