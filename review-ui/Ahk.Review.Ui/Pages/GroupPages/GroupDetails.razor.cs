using Ahk.Review.Ui.Models;
using Ahk.Review.Ui.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using Newtonsoft.Json;

namespace Ahk.Review.Ui.Pages.GroupPages
{
    [Authorize]
    public partial class GroupDetails : ComponentBase
    {
        [Parameter]
        public string GroupId { get; set; }

        [Inject]
        private GroupService GroupService { get; set; }
        [Inject]
        private SubjectService SubjectService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private Group group = new Group();
        private List<Student> students = new List<Student>();
        private List<Teacher> teachers = new List<Teacher>();

        protected override async void OnInitialized()
        {
            group = await GroupService.GetGroupAsync(SubjectService.TenantCode, GroupId);
            students = await GroupService.ListStudentsInGroup(GroupId);
            teachers = await GroupService.ListTeachersInGroup(GroupId);

            StateHasChanged();
        }

        private void AddTeacher()
        {
            NavigationManager.NavigateTo($"/add-user/{GroupId}/teacher");
        }

        private async Task RemoveTeacherFromGroup(int teacherId)
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to remove this teacher from the group?");

            if (confirmed)
            {
                teachers.Remove(teachers.Where(t => t.Id == teacherId).FirstOrDefault());
                await GroupService.RemoveTeacherFromGroup(GroupId, teacherId.ToString());

                StateHasChanged();
            }
        }

        private void AddStudent()
        {
            NavigationManager.NavigateTo($"/add-user/{GroupId}/student");
        }

        private async Task RemoveStudentFromGroup(int studentId)
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to remove this student from the group?");

            if (confirmed)
            {
                students.Remove(students.Where(s => s.Id == studentId).FirstOrDefault());
                await GroupService.RemoveStudentFromGroup(GroupId, studentId.ToString());

                StateHasChanged();
            }
        }
    }
}
