using System.Collections.Generic;
using System.Threading.Tasks;
using Ahk.GradeManagement.Data;
using Ahk.GradeManagement.Data.Entities;
using Ahk.GradeManagement.ResultProcessing.Dto;
using Ahk.GradeManagement.SetGrade;

namespace Ahk.GradeManagement.Services
{
    public interface IGradeService
    {
        AhkDbContext Context { get; set; }
        Task AddGradeAsync(Grade value);
        Task<Grade> GetLastResultOfAsync(string neptun, string gitHubRepoName, int gitHubPrNumber);
        Task<IReadOnlyCollection<Grade>> ListConfirmedWithRepositoryPrefixAsync(string subject, string repoPrefix);
        Student FindStudent(string neptun);
        Assignment FindAssignment(AhkTaskResult[] results);
        int FindAssignmentId(AhkTaskResult[] results);
        List<Point> GetPoints(AhkTaskResult[] results);
        Task UpdateGradeAsync(Grade value);
        Task SetGradeAsync(SetGradeEvent data, Grade grade);
    }
}
