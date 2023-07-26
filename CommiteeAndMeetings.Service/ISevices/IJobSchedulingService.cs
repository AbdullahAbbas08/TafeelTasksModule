using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IJobSchedulingService
    {
        Task SendReminderMail(string code);
        Task TaskEscalation(); 
    }
}
