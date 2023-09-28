using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ahk.GradeManagement.Data.Entities;
using Ahk.GradeManagement.Services.AssignmentService;
using Azure.Core;

using DTOs;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Ahk.GradeManagement.Functions.Assignments
{
    public class CreateAssignmentFunction
    {
        private readonly ILogger _logger;
        private readonly IAssignmentService service;

        public CreateAssignmentFunction(ILoggerFactory loggerFactory, IAssignmentService service)
        {
            _logger = loggerFactory.CreateLogger<CreateAssignmentFunction>();
            this.service = service;
        }

        [Function("create-assignment")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "create-assignment/{subject}")] HttpRequestData req, string subject,
            [FromBody] AssignmentDTO assignmentDTO)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var exercises = assignmentDTO.Exercises.Select(eDTO => 
            {
                return new Exercise
                {
                    Name = eDTO.Name,
                    AvailablePoints = eDTO.AvailablePoints,
                };
            }).ToList();

            var assignment = new Assignment
            {
                Name = assignmentDTO.Name,
                DeadLine = assignmentDTO.DeadLine,
                ClassroomAssignment = assignmentDTO.ClassroomAssignment,
                Exercises = exercises,
            };

            await service.SaveAssignmentAsync(assignment, Uri.UnescapeDataString(subject));
            return new OkResult();
        }
    }
}
