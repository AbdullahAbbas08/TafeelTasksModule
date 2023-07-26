using CommiteeAndMeetings.Service.ISevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.HangfireJobs
{
    public class SendMailWithSecondReminderJob
    {
        private IJobSchedulingService jobService;
        public SendMailWithSecondReminderJob(IJobSchedulingService _jobService)
        {
            jobService = _jobService;
        }
        public Task Run()
        {
            jobService.SendReminderMail("NumberOfDaysToSecondReminder");
            return Task.CompletedTask;
        }
    }
}
