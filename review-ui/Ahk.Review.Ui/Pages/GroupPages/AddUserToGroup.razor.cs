using Ahk.Review.Ui.Models;
using Ahk.Review.Ui.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Ahk.Review.Ui.Pages.GroupPages;

[Authorize]
public partial class AddUserToGroup : ComponentBase
{
    [Parameter]
    public string GroupId { get; set; }
    [Parameter]
    public string UserType { get; set; }

    [Inject]
    private GroupService GroupService { get; set; }
    [Inject]
    private SubjectService SubjectService { get; set; }
    [Inject]
    private NavigationManager NavigationManager { get; set; }

    private string name;
    private string neptun;
    private string eduEmail;
    private string githubUser;

    private async void Submit()
    {
        if (UserType == "teacher")
        {
            Teacher teacher = new Teacher
            {
                Name = name,
                Neptun = neptun,
                EduEmail = eduEmail,
                GithubUser = githubUser,
            };

            await GroupService.AddTeacherToGroup(SubjectService.TenantCode, GroupId, teacher);
        }

        if (UserType == "student")
        {
            Student student = new Student
            {
                Name = name,
                Neptun = neptun,
                EduEmail = eduEmail,
                GithubUser = githubUser,
            };

            await GroupService.AddStudentToGroup(SubjectService.TenantCode, GroupId, student);
        }

        NavigationManager.NavigateTo($"/group-details/{GroupId}");
    }
}
