using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class SubmissionInfoDTO
    {
        public string Repository { get; set; }
        public string Neptun { get; set; }
        public List<string> Branches { get; set; }
        public List<PullRequestStatusDTO> PullRequests { get; set; }
        public WorkflowRunsStatusDTO WorkflowRuns { get; set; }
        public string RepositoryUrl { get; set; }
        public string Grade { get; set; }
    }
}
