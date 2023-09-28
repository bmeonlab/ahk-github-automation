using DTOs;

namespace Ahk.Review.Ui
{ 

    public class SubmissionInfo
    {
        public SubmissionInfo(string AssignmentName, string Repository, string Neptun,
                              List<string> Branches, List<PullRequestStatusDTO> PullRequests,
                              WorkflowRunsStatusDTO WorkflowRuns, IReadOnlyDictionary<string, double>? points, List<StatusEventBaseDTO> Events, bool showDetails)
        {
            this.AssignmentName = AssignmentName;
            this.Repository = Repository;
            this.Neptun = Neptun;
            this.Branches = Branches;
            this.PullRequests = PullRequests;
            this.WorkflowRuns = WorkflowRuns;
            this.Grade = getGradeAsString(points);
            this.RepositoryUrl = $"https://github.com/{Repository}";
            this.Events = Events;
            this.ShowDetails = showDetails;
        }

        public string AssignmentName { get; set; }
        public string Repository { get; set; }
        public string Neptun { get; set; }
        public List<string> Branches { get; set; }
        public List<PullRequestStatusDTO> PullRequests { get; set; }
        public WorkflowRunsStatusDTO WorkflowRuns { get; set; }
        public string RepositoryUrl { get; set; }
        public string Grade { get; set; }
        public List<StatusEventBaseDTO> Events { get; set; }
        public bool ShowDetails { get; set; }

        private static string getGradeAsString(IReadOnlyDictionary<string, double>? points)
        {
            if (points is null)
                return string.Empty;

            return string.Join(" ", points.Values);
        }
    }
}
