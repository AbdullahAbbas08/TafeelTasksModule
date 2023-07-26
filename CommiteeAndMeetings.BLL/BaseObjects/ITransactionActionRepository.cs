using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.Views;
using Models.ProjectionModels;
using System.Collections.Generic;

namespace CommiteeAndMeetings.BLL.BaseObjects
{
    public interface ITransactionActionRepository : IRepository<TransactionAction>
    {
        IEnumerable<Vw_TransactionBoxes_Inbox> GetInbox(int page, int pageSize, bool count, int? organizationId, int? userId, bool isEmployee, string searchtxt, int filterId, int? userRoleId, int? CommitteId, InboxFilterFieldsDTo inboxFilterFields = null, bool isForAllSystem = false);
        long GetInboxCount(int page, int pageSize, bool count, int? organizationId, int? userId, bool isEmployee, string searchtxt, int filterId, int userRoleId, int? CommitteId, InboxFilterFieldsDTo inboxFilterFields = null, bool isForAllSystem = false);
        IEnumerable<Vw_TransactionBoxes> GetOutbox(int page, int pageSize, bool count, int organizationId, int userId, bool isEmployee, string searchtxt, int filterId, int UserRoleId, int? CommitteId, InboxFilterFieldsDTo inboxFilterFields = null);
        public long GetOutboxCount(int page, int pageSize, bool count, int organizationId, int userId, bool isEmployee, string searchtxt, int filterId, int UserRoleId, int? CommitteId, InboxFilterFieldsDTo inboxFilterFields = null);
        long GetOutboxConfirmationCount(int page, int pageSize, bool isCount, int organizationId, int userId, bool isEmployee, string searchText, int filterId, int userRoleId, InboxFilterFieldsDTo inboxFilterFields);
        IEnumerable<Vw_TransactionBoxes> GetOutConfirmationbox(int page, int pageSize, bool isCount, int organizationId, int userId, bool isEmployee, string searchText, int filterId, int userRoleId, InboxFilterFieldsDTo inboxFilterFields);
        IEnumerable<Vw_Attachments> GetTransactionActionRecipientsAttachments(long transactionId, int transactionActionId, int transactionActionRecipientId, bool ShowRelatedAttachments);
    }
}
