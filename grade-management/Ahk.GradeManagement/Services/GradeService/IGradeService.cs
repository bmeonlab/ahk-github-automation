using System.Collections.Generic;
using System.Threading.Tasks;
using Ahk.GradeManagement.Data;
using Ahk.GradeManagement.Data.Entities;
using Ahk.GradeManagement.ResultProcessing.Dto;

namespace Ahk.GradeManagement.Services
{
    public interface IGradeService
    {
        AhkDbContext Context { get; set; }
        Task AddGradeAsync(Grade value);
        Task<Grade> GetLastResultOfAsync(string neptun, string gitHubRepoName, int gitHubPrNumber);
        Task<IReadOnlyCollection<Grade>> ListConfirmedWithRepositoryPrefixAsync(string repoPrefix);
        Student FindStudentAsync(string neptun);
        Assignment FindAssignment(AhkTaskResult[] results);
    }
}
