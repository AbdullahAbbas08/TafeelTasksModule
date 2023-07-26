using CommiteeAndMeetings.Service.ISevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.HangfireJobs
{
    public class TaskEscalationJob
    {
        private IJobSchedulingService jobService;
        public TaskEscalationJob(IJobSchedulingService _jobService)
        {
            jobService = _jobService;
        }
        public Task Run()
        {
            jobService.TaskEscalation();
            return Task.CompletedTask;
        }
    }
}
