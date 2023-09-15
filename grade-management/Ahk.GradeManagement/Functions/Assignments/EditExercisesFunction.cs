using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Ahk.GradeManagement.Data.Entities;
using Ahk.GradeManagement.Services.AssignmentService;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Ahk.GradeManagement.Functions.Assignments
{
    public class EditExercisesFunction
    {
        private readonly ILogger _logger;
        private readonly IAssignmentService assignmentService;

        public EditExercisesFunction(ILoggerFactory loggerFactory, IAssignmentService assignmentService)
        {
            _logger = loggerFactory.CreateLogger<EditAssignmentFunction>();
            this.assignmentService = assignmentService;
        }

        [Function("edit-exercises")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "edit-exercises")] HttpRequestData req,
           [FromBody] List<ExerciseDTO> exerciseDTOs )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            List<Exercise> exercises = new List<Exercise>();
            foreach (var dto in exerciseDTOs)
            {
                exercises.Add(new Exercise()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    AvailablePoints = dto.AvailablePoints,
                });
            }

            await assignmentService.UpdateExercisesAsync(exercises);

            return new OkResult();
        }
    }
}
