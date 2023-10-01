using Ahk.Review.Ui.Models;
using Ahk.Review.Ui.Services;
using Microsoft.AspNetCore.Components;

namespace Ahk.Review.Ui.Pages.SubjectPages
{
    public partial class CreateSubject : ComponentBase
    {
        [Inject]
        private SubjectService SubjectService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private string name;
        private string subjectCode;
        private string semester;
        private string githubOrg;
        private string ahkConfig;

        private async void Submit()
        {
            Subject subject = new Subject
            {
                Name = name,
                SubjectCode = subjectCode,
                Semester = semester,
                GithubOrg = githubOrg,
                AhkConfig = ahkConfig,
            };

            await SubjectService.CreateSubjectAsync(subject);

            NavigationManager.NavigateTo("/", true);
        }
    }
}
