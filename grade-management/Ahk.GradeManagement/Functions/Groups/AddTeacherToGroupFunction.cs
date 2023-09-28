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
    public class AddTeacherToGroupFunction
    {
        private readonly ILogger _logger;
        private readonly IGroupService groupService;

        public AddTeacherToGroupFunction(ILoggerFactory loggerFactory, IGroupService groupService)
        {
            _logger = loggerFactory.CreateLogger<AddTeacherToGroupFunction>();
            this.groupService = groupService;
        }

        [Function("add-teacher")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "add-teacher/{subject}/{groupId}")] HttpRequestData req, string subject, string groupId,
            [FromBody] TeacherDTO teacherDTO)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var teacher = new Teacher()
            {
                Name = teacherDTO.Name,
                Neptun = teacherDTO.Neptun,
                EduEmail = teacherDTO.EduEmail,
                GithubUser = teacherDTO.GithubUser,
            };

            await groupService.AddTeacherToGroupAsync(Uri.UnescapeDataString(subject), groupId, teacher);

            return new OkResult();
        }
    }
}
