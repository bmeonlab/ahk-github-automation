using Ahk.Review.Ui.Models;
using Ahk.Review.Ui.Services;

using Microsoft.AspNetCore.Components;

namespace Ahk.Review.Ui.Pages.AssignmentPages
{
    public partial class AssignmentDetails : ComponentBase
    {
        [Parameter]
        public string AssignmentId { get; set; }

        [Inject]
        private AssignmentService AssignmentService { get; set; }
        [Inject]
        private SubjectService SubjectService { get; set; }

        private Assignment assignment;
        private string subjectName;
        private List<Exercise> exerciseList;

        protected override async void OnInitialized()
        {
            assignment = await AssignmentService.GetAssignmentAsync(SubjectService.CurrentTenant.SubjectCode, AssignmentId);
            subjectName = SubjectService.CurrentTenant.Name;
            exerciseList = await AssignmentService.GetExercisesAsync(SubjectService.CurrentTenant.SubjectCode, AssignmentId);

            StateHasChanged();
        }
    }
}
