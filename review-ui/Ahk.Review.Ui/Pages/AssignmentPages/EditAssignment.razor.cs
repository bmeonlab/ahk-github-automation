using Ahk.Review.Ui.Models;
using Ahk.Review.Ui.Services;
using Microsoft.AspNetCore.Components;

using Newtonsoft.Json;

namespace Ahk.Review.Ui.Pages.AssignmentPages
{
    public partial class EditAssignment : ComponentBase
    {
        [Parameter]
        public string Subject { get; set; }
        [Parameter]
        public string AssignmentId { get; set; }

        private Assignment assignment = new Assignment();
        private string classroomAssignment;
        private DateTime? date;
        private TimeSpan? time;
        private List<Exercise> exercises = new List<Exercise>();

        [Inject]
        private AssignmentService AssignmentService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override async void OnInitialized()
        {
            Subject = Uri.UnescapeDataString(Subject);
            assignment = await AssignmentService.GetAssignmentAsync(Subject, AssignmentId);
            classroomAssignment = assignment.ClassroomAssignment.ToString();
            date = assignment.DeadLine.Date;
            time = assignment.DeadLine.TimeOfDay;
            exercises = await AssignmentService.GetExercisesAsync(Subject, AssignmentId);

            StateHasChanged();
        }

        private async Task SubmitAsync()
        {
            Assignment assignmentUpdate = assignment;
            assignmentUpdate.ClassroomAssignment = new Uri(classroomAssignment);

            DateTime dateForUpdate = (DateTime)date;
            TimeSpan timeForUpdate = (TimeSpan)time;

            assignmentUpdate.DeadLine = new DateTimeOffset(dateForUpdate.AddSeconds(timeForUpdate.TotalSeconds));

            List<Exercise> exercisesUpdate = exercises;

            await AssignmentService.EditAssignmentAsync(assignmentUpdate, exercisesUpdate);

            NavigationManager.NavigateTo($"/subject-details");
        }
    }
}
