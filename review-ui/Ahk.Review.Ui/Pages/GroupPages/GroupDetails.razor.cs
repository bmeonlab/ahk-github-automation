using Ahk.Review.Ui.Models;
using Ahk.Review.Ui.Services;

using Microsoft.AspNetCore.Components;

namespace Ahk.Review.Ui.Pages.GroupPages
{
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

        private Group group;
        private List<Student> students;
        private List<Teacher> teachers;

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
            teachers.Remove(teachers.Where(t => t.Id == teacherId).FirstOrDefault());
            await GroupService.RemoveTeacherFromGroup(GroupId, teacherId.ToString());

            StateHasChanged();
        }

        private void AddStudent()
        {
            NavigationManager.NavigateTo($"/add-user/{GroupId}/student");
        }

        private async Task RemoveStudentFromGroup(int studentId)
        {
            students.Remove(students.Where(s => s.Id == studentId).FirstOrDefault());
            await GroupService.RemoveStudentFromGroup(GroupId, studentId.ToString());

            StateHasChanged();
        }
    }
}
