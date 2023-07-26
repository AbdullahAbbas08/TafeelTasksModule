using CommiteeAndMeetings.Service.ISevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.HangfireJobs
{
    public class SendMailWithFirstReminderJob 
    {
        private IJobSchedulingService jobService;
        public SendMailWithFirstReminderJob(IJobSchedulingService _jobService)
        {
            jobService = _jobService;
        }
        public Task Run()
        {
            jobService.SendReminderMail("NumberOfDaysToFirstReminder");
            return Task.CompletedTask;
        }
    }
}
