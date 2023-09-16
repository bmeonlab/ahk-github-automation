using Ahk.Review.Ui.Models;
using Ahk.Review.Ui.Services;
using Microsoft.AspNetCore.Components;

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
        private string classroomAssignment;
        private DateTime? date;
        private TimeSpan? time;


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

            await AssignmentService.PostDataAsync(assignment);
            NavigationManager.NavigateTo("/subject-details");
        }
    }
}
