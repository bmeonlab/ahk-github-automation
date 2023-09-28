using System;
using System.Net;
using System.Threading.Tasks;

using Ahk.GradeManagement.Data.Entities;
using Ahk.GradeManagement.Services.GroupService;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Ahk.GradeManagement.Functions.Groups
{
    public class AddStudentToGroupFunction
    {
        private readonly ILogger _logger;
        private readonly IGroupService groupService;

        public AddStudentToGroupFunction(ILoggerFactory loggerFactory, IGroupService groupService)
        {
            _logger = loggerFactory.CreateLogger<AddStudentToGroupFunction>();
            this.groupService = groupService;
        }

        [Function("add-student")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "add-student/{subject}/{groupId}")] HttpRequestData req, string subject, string groupId,
            [FromBody] StudentDTO studentDTO)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var student = new Student()
            {
                Name = studentDTO.Name,
                Neptun = studentDTO.Neptun,
                EduEmail = studentDTO.EduEmail,
                GithubUser = studentDTO.GithubUser,
            };

            await groupService.AddStudentToGroupAsync(Uri.UnescapeDataString(subject), groupId, student);

            return new OkResult();
        }
    }
}
