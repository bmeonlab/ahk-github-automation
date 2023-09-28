using Ahk.Review.Ui.Models;
using Ahk.Review.Ui.Services;
using Microsoft.AspNetCore.Components;

using System.ComponentModel.DataAnnotations;

namespace Ahk.Review.Ui.Pages.AssignmentPages
{
    public partial class CreateAssignment : ComponentBase
    {
        [Inject]
        private AssignmentService AssignmentService { get; set; }
        [Inject]
        private SubjectService SubjectService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private string assignmentName;
        [Url]
        private string classroomAssignment;
        private DateTime? date;
        private TimeSpan? time;
        private string exerciseName;
        private string availablePoints;
        private List<Exercise> exercises = new List<Exercise>();


        protected override void OnInitialized()
        {
        }

        private async void SubmitAssignment()
        {
            DateTime dateForUpdate = (DateTime)date;
            TimeSpan timeForUpdate = (TimeSpan)time;

            Assignment assignment = new Assignment
            {
                Name = assignmentName,
                DeadLine = new DateTimeOffset(dateForUpdate.AddSeconds(timeForUpdate.TotalSeconds)),
                ClassroomAssignment = new Uri(classroomAssignment),
                SubjectId = SubjectService.CurrentTenant.Id,
            };

            await AssignmentService.CreateAssingmentAsync(SubjectService.TenantCode, assignment, exercises);
            NavigationManager.NavigateTo("/subject-details");
        }

        private void AddExercise()
        {
            Exercise exercise = new Exercise
            {
                Name = exerciseName,
                AvailablePoints = int.Parse(availablePoints),
            };

            exercises.Add(exercise);

            exerciseName = "";
            availablePoints = "";

            StateHasChanged();
        }
    }
}
