using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;
using LinqHelper;
using System.Collections.Generic;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommitteeNotificationService : IBusinessService<CommitteeNotification, CommitteeNotificationDTO>
    {
        int GetNotificationCount(int userId);
        DataSourceResult<CommitteeNotificationDTO> GetNotificationList(int userId, int take, int skip);
        CommitteeNotificationDTO GetNotificationRead(int committeeNotificationId);
        void insertNotification();
    }
}

