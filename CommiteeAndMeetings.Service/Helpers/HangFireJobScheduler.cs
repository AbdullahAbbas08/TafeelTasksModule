using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.Service.HangfireJobs;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.Helpers
{
    public class HangFireJobScheduler
    {
        public static void RegisterSchedulerRecurringJobs(List<CommitteeMeetingSystemSetting> Jobs)
        {
            var _Job = Jobs.FirstOrDefault(a => a.SystemSettingCode == "TimeToExecuteFirstReminder");
            if(_Job != null && _Job.SystemSettingValue != null)
            {
                RecurringJob.AddOrUpdate<SendMailWithFirstReminderJob>(
                    nameof(SendMailWithFirstReminderJob),
                    job => job.Run(),
                   _Job.SystemSettingValue);
            }
            var _Job2 = Jobs.FirstOrDefault(a => a.SystemSettingCode == "TimeToExecuteSecondReminder");
            if (_Job2 != null && _Job2.SystemSettingValue != null)
            {
                RecurringJob.AddOrUpdate<SendMailWithSecondReminderJob>(
                    nameof(SendMailWithSecondReminderJob),
                    job => job.Run(),
                   _Job2.SystemSettingValue);
            }

            var _Job3 = Jobs.FirstOrDefault(a => a.SystemSettingCode == "AutomaticEscalationTime");
            if (_Job3 != null && _Job3.SystemSettingValue != null)
            {
                RecurringJob.AddOrUpdate<TaskEscalationJob>(
                    nameof(TaskEscalationJob),
                    job => job.Run(),
                   _Job3.SystemSettingValue);
            }
        }
    }
}
