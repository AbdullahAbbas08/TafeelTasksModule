using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.Views;
using IHelperServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.BLL.BaseObjects
{
    public class TransactionActionRepository : BaseRepository<TransactionAction>, ITransactionActionRepository
    {
        private MasarContext Db { get; set; }
        ISessionServices _sessionServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        //  public ITransactionActionRecipientRepository TransactionActionRecipientRepository { get; set; }
        public TransactionActionRepository(MasarContext mainDbContext, ISessionServices sessionServices, IHttpContextAccessor httpContextAccessor) : base(mainDbContext, sessionServices, httpContextAccessor)
        {
            Db = mainDbContext;
            _httpContextAccessor = httpContextAccessor;
            //   TransactionActionRecipientRepository = new TransactionActionRecipientRepository(mainDbContext, sessionServices);
        }

        #region Inbox
        public IEnumerable<Vw_TransactionBoxes_Inbox> GetInbox(int page, int pageSize, bool count, int? organizationId, int? userId, bool isEmployee, string searchtxt, int filterId, int? userRoleId, int? CommitteId, InboxFilterFieldsDTo inboxFilterFields = null, bool isForAllSystem = false)
        {
            try
            {
                int? fromIncomingOrganizationId = inboxFilterFields?.FromIncomingOrganizationId;
                bool? IsEmployeeFilter = inboxFilterFields?.IsEmployeeFilter;
                int? fromId = inboxFilterFields?.FromId;
                int? Case = inboxFilterFields?.FilterCase;

                IQueryable<Vw_TransactionBoxes_Inbox> inbox = this._dbContext.Vw_TransactionBoxes_Inbox.FromSqlRaw($"exec sp_GetInbox {((page - 1) * pageSize)},{pageSize},{count},{organizationId},{userId},{isEmployee},{searchtxt},{filterId} ,{userRoleId},{fromIncomingOrganizationId},{inboxFilterFields?.From},{inboxFilterFields?.To},{IsEmployeeFilter},{fromId},{Case},{isForAllSystem},{CommitteId}");
                return inbox;

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public long GetInboxCount(int page, int pageSize, bool count, int? organizationId, int? userId, bool isEmployee, string searchtxt, int filterId, int userRoleId, int? CommitteId, InboxFilterFieldsDTo inboxFilterFields = null, bool isForAllSystem = false)
        {
            int? fromIncomingOrganizationId = inboxFilterFields?.FromIncomingOrganizationId;
            bool? IsEmployeeFilter = inboxFilterFields?.IsEmployeeFilter;
            int? fromId = inboxFilterFields?.FromId;
            int? Case = inboxFilterFields?.FilterCase;

            var x = this._dbContext.COUNTS.FromSqlRaw($"exec sp_GetInbox {((page - 1) * pageSize)},{pageSize},{count},{organizationId},{userId},{isEmployee},{searchtxt},{filterId},{userRoleId},{fromIncomingOrganizationId},{inboxFilterFields?.From},{inboxFilterFields?.To},{IsEmployeeFilter},{fromId},{Case},{isForAllSystem},{CommitteId}").FirstOrDefault().CNT;
            return x;
        }
        #endregion
        public IEnumerable<TransactionByTypeReportDTO> GetTransactionByTypeReport(int userId, int OrganizationId, int userRoleId, DateTimeOffset? fROM_DATE, DateTimeOffset? tO_DATE)
        {
            var from = (fROM_DATE == null) ? "" : fROM_DATE.ToString();
            var to = (tO_DATE == null) ? "" : tO_DATE.ToString();
            return this._dbContext.TransactionByTypeReportDTO.FromSqlRaw($"exec GetTransactionByTypeReport {userId},{OrganizationId},{userRoleId},{from},{to}");
        }





        #region Outbox
        public IEnumerable<Vw_TransactionBoxes> GetOutbox(int page, int pageSize, bool count, int organizationId, int userId, bool isEmployee, string searchtxt, int filterId, int UserRoleId, int? CommitteId, InboxFilterFieldsDTo inboxFilterFields = null)
        {
            try
            {
                int? fromIncomingOrganizationId = inboxFilterFields?.FromIncomingOrganizationId;
                bool? IsEmployeeFilter = inboxFilterFields?.IsEmployeeFilter;
                int? fromId = inboxFilterFields?.FromId;
                int? Case = inboxFilterFields?.FilterCase;
                var Data = this._dbContext.ViewTransactionsView.FromSqlRaw($"exec sp_GetOutbox {((page - 1) * pageSize)},{pageSize},{count},{organizationId},{userId},{isEmployee},{searchtxt},{filterId},{UserRoleId},{fromIncomingOrganizationId},{inboxFilterFields?.From},{inboxFilterFields?.To},{IsEmployeeFilter},{fromId},{Case},{CommitteId}");
                return Data;

            }
            catch (Exception)
            {

                throw;
            }


        }

        public long GetOutboxCount(int page, int pageSize, bool count, int organizationId, int userId, bool isEmployee, string searchtxt, int filterId, int UserRoleId, int? CommitteId, InboxFilterFieldsDTo inboxFilterFields = null)
        {
            try
            {
                int? fromIncomingOrganizationId = inboxFilterFields?.FromIncomingOrganizationId;
                bool? IsEmployeeFilter = inboxFilterFields?.IsEmployeeFilter;
                int? fromId = inboxFilterFields?.FromId;
                int? Case = inboxFilterFields?.FilterCase;


                return this._dbContext.COUNTS.FromSqlRaw($"exec sp_GetOutbox {((page - 1) * pageSize)},{pageSize},{count},{organizationId},{userId},{isEmployee},{searchtxt},{filterId},{UserRoleId},{fromIncomingOrganizationId},{inboxFilterFields?.From},{inboxFilterFields?.To},{IsEmployeeFilter},{fromId},{Case},{CommitteId}").FirstOrDefault().CNT;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void UpdateMaxTransactionActionReceipientId(long TransactionId, bool IsEmployee, int? FromOrganizationId, int? FromUserId)
        {
            try
            {
                _dbContext.COUNTS.FromSqlRaw($"exec sp_UpdateMaxTransactionActionReceipientId {TransactionId},{IsEmployee},{FromOrganizationId},{FromUserId}").ToList();
            }
            catch (Exception ex)
            {

            }
        }

        public long GetOutboxConfirmationCount(int page, int pageSize, bool isCount, int organizationId, int userId, bool isEmployee, string searchText, int filterId, int userRoleId, InboxFilterFieldsDTo inboxFilterFields)
        {
            try
            {
                int? fromIncomingOrganizationId = inboxFilterFields?.FromIncomingOrganizationId;
                bool? IsEmployeeFilter = inboxFilterFields?.IsEmployeeFilter;
                int? fromId = inboxFilterFields?.FromId;
                int? Case = inboxFilterFields?.FilterCase;


                return this._dbContext.COUNTS.FromSqlRaw($"exec sp_GetOutConfirmationbox {((page - 1) * pageSize)},{pageSize},{isCount},{organizationId},{userId},{isEmployee},{searchText},{filterId},{userRoleId},{fromIncomingOrganizationId},{inboxFilterFields?.From},{inboxFilterFields?.To},{IsEmployeeFilter},{fromId},{Case}").FirstOrDefault().CNT;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Vw_TransactionBoxes> GetOutConfirmationbox(int page, int pageSize, bool isCount, int organizationId, int userId, bool isEmployee, string searchText, int filterId, int userRoleId, InboxFilterFieldsDTo inboxFilterFields)
        {
            try
            {
                int? fromIncomingOrganizationId = inboxFilterFields?.FromIncomingOrganizationId;
                bool? IsEmployeeFilter = inboxFilterFields?.IsEmployeeFilter;
                int? fromId = inboxFilterFields?.FromId;
                int? Case = inboxFilterFields?.FilterCase;

                var Data = this._dbContext.ViewTransactionsView.FromSqlRaw($"exec sp_GetOutConfirmationbox {((page - 1) * pageSize)},{pageSize},{isCount},{organizationId},{userId},{isEmployee},{searchText},{filterId},{userRoleId},{fromIncomingOrganizationId},{inboxFilterFields?.From},{inboxFilterFields?.To},{IsEmployeeFilter},{fromId},{Case}");
                return Data;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IEnumerable<Vw_Attachments> GetTransactionActionRecipientsAttachments(long transactionId, int transactionActionId, int transactionActionRecipientId, bool ShowRelatedAttachments)
        {
            try
            {
                return _dbContext.attachmentsViews.FromSqlRaw($"exec sp_GetTransactionActionRecipientsAttachments {transactionId},{transactionActionId},{transactionActionRecipientId},{ShowRelatedAttachments},{_sessionServices.UserId}").ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
    }
}
