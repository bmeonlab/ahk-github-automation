using Ahk.Review.Ui.Models;
using Ahk.Review.Ui.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Ahk.Review.Ui.Pages.SubjectPages
{
    [Authorize]
    public partial class EditSubject : ComponentBase, IDisposable
    {
        [Parameter]
        public string SubjectId { get; set; }

        [Inject]
        private SubjectService SubjectService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private Subject subject;

        protected override void OnInitialized()
        {
            subject = SubjectService.CurrentTenant;

            SubjectService.OnChange += StateHasChanged;
        }

        private async void Submit()
        {
            await SubjectService.EditSubjectAsync(subject);

            NavigationManager.NavigateTo("/subject-details");
        }

        public void Dispose()
        {
            SubjectService.OnChange -= StateHasChanged;
        }
    }
}
