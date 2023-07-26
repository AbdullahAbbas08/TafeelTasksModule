using CommiteeAndMeetings.DAL.CommiteeDTO;
using System.Collections.Generic;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface IUpdateTaskLogService
    {
        List<UpdateTaskLogDTO> GetTaskUpdateslog(int CommiteeTaskId);
    }
}
