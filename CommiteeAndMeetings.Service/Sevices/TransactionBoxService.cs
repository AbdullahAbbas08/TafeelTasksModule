using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL;
using CommiteeAndMeetings.BLL.BaseObjects;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.BLL.Hosting;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.DAL.Views;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using DbContexts.MasarContext.ProjectionModels;
using DotLiquid;
using EFCore.BulkExtensions;
using IHelperServices.Models;
using LinqHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Models;
using Models.Enums;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.AcroFields;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class TransactionBoxService : BusinessService<TransactionAction, TransactionBoxDTO>, ITransactionBoxService
    {
        public string culture = string.Empty;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDataProtectService _dataProtectService;
        IHelperServices.ISessionServices _sessionServices;
        //   private readonly ITransactionActionRepository transactionActionRepository;
        public readonly MasarContext _context;
        private IHttpContextAccessor _contextAccessor;
        ISystemSettingsService _SystemSettingsService;
        protected readonly IHelperServices.ISessionServices _SessionServices;
        private readonly IDocumentService _IDocumentServices;
        public TransactionBoxService(IUnitOfWork unitOfWork, IMapper mapper,
            IDataProtectService dataProtectService, IStringLocalizer stringLocalizer, 
            IHelperServices.ISessionServices sessionSevices, ISystemSettingsService systemSettingsService,
            ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings,
            IHttpContextAccessor httpContextAccessor, IDocumentService documentService)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _context = new MasarContext();
            //    transactionActionRepository = _UnitOfWork.GetRepository<TransactionAction>(true) as ITransactionActionRepository;
            _unitOfWork = unitOfWork;
            _dataProtectService = dataProtectService;
            _sessionServices = sessionServices;
            _SystemSettingsService = systemSettingsService;
            culture = _sessionServices.Culture;
            _SessionServices = sessionSevices;
            _contextAccessor = httpContextAccessor;
            _IDocumentServices = documentService;
        }

        public string GetTransactionStatus(long Transactionid, string Culutre)
        {
            string transactionStatus = string.Empty;

            try
            {
                transactionStatus = this._context.STRINGS.FromSqlRaw($"exec [dbo].[sp_CheckTransactionStatus] {Transactionid}, {Culutre}").AsEnumerable().FirstOrDefault().valAsString;
            }

            catch (Exception ex)
            {
                return "";
            }

            return transactionStatus;
        }

        public DataSourceResult<TransactionBoxDTO> GetboxDetails(string BoxType, string searchText, int page, int pageSize, bool isCount, int? CommitteId, bool isEmployee = true, int filterId = 0, InboxFilterFieldsDTo inboxFilterFields = null)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                searchText = "";
            }

            BoxType = BoxType.ToUpper();

            switch (BoxType)
            {
                case "OUTBOX": return GetOutboxDetails(searchText, page, pageSize, isCount, CommitteId, isEmployee, filterId, inboxFilterFields);
                case "INBOX": return GetInboxDetails(searchText, page, pageSize, isCount, CommitteId, isEmployee, filterId, inboxFilterFields);
                //case "SAVED": return GetInboxSavedDetails(searchText, page, pageSize, isCount, isEmployee, filterId, inboxFilterFields);
                //case "WITHDRAWAL": return GetInboxWithdrawalDetails(searchText, page, pageSize, isCount, isEmployee, filterId, inboxFilterFields);
                //case "DRAFT": return GetDraftTransactions(searchText, page, pageSize, isCount, true, filterId);
                //case "RECEIVEOUTGOING": return GetReceivingOutgoingDelegation(isEmployee, searchText, page, pageSize, isCount, filterId);
                //case "OUTGOING": return outDelegation(isEmployee, searchText, page, pageSize, isCount, filterId);
                //case "EDITOUTGOING": return Edit_outDelegation(isEmployee, searchText, page, pageSize, isCount, filterId);

                default: return null;
            };
        }
        private DataSourceResult<TransactionBoxDTO> GetInboxDetails(string searchText, int page, int pageSize, bool isCount, int? CommitteId, bool isEmployee = true, int filterId = 0, InboxFilterFieldsDTo inboxFilterFields = null)
        {
            int userId = _sessionServices.UserId.Value;
            if (inboxFilterFields.SearchInSpecificUser_UserId != null)
            {
                userId = inboxFilterFields.SearchInSpecificUser_UserId.Value;
                isEmployee = true;
            }
            int organizationId = 0;
            int userRoleId = 0;

            long countData = 0;
            int? fromIncomingOrganizationId = inboxFilterFields?.FromIncomingOrganizationId;
            bool? IsEmployeeFilter = inboxFilterFields?.IsEmployeeFilter;
            int? fromId = inboxFilterFields?.FromId;
            int? Case = inboxFilterFields?.FilterCase;
            var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
            var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
            userRoleId = userRole.UserRoleId;
            if (isCount)
            {


                countData = this._context.COUNTS.FromSqlInterpolated($"exec sp_GetInbox {((page - 1) * pageSize)},{pageSize},{isCount},{organizationId},{userId},{isEmployee},{searchText},{filterId},{userRoleId},{fromIncomingOrganizationId},{inboxFilterFields?.From},{inboxFilterFields?.To},{IsEmployeeFilter},{fromId},{Case},{true},{true},{CommitteId},{null},{null}").AsEnumerable().FirstOrDefault().CNT;
                //  countData = transactionActionRepository.GetInboxCount(page, pageSize, isCount, organizationId, userId, isEmployee, searchText, filterId, userRoleId, CommitteId, inboxFilterFields);
            }

            //   this._context.COUNTS.FromSqlInterpolated($"exec sp_GetInbox {((page - 1) * pageSize)},{pageSize},{false},{organizationId},{userId},{isEmployee},{searchText},{filterId},{userRoleId},{fromIncomingOrganizationId},{inboxFilterFields?.From},{inboxFilterFields?.To},{IsEmployeeFilter},{fromId},{Case},{true},{CommitteId}").FirstOrDefault().CNT;
            //  IQueryable<Vw_TransactionBoxes_Inbox> inbox = this._context.Vw_TransactionBoxes_Inbox.FromSqlInterpolated($"exec sp_GetInbox {((page - 1) * pageSize)},{pageSize},{false},{"''"},{userId},{isEmployee},{((searchText == "") ? "''" : searchText)},{filterId} ,{"''"},{"''"},{"''"},{"''"},{"''"},{"''"},{Case},{true},{CommitteId}");
            IQueryable<Vw_TransactionBoxes_Inbox> inbox = this._context.Vw_TransactionBoxes_Inbox.FromSqlInterpolated($"exec sp_GetInbox {((page - 1) * pageSize)},{pageSize},{false},{organizationId},{userId},{isEmployee},{searchText},{filterId},{userRoleId},{fromIncomingOrganizationId},{inboxFilterFields?.From},{inboxFilterFields?.To},{IsEmployeeFilter},{fromId},{Case},{false},{true},{CommitteId},{null},{null}");
            List<Vw_TransactionBoxes_Inbox> transactionViewData = inbox.ToList();
            try
            {
                IEnumerable<TransactionBoxDTO> inboxData = transactionViewData.Select(x => new TransactionBoxDTO
                {
                    hasCorrespondence = x.hasCorrespondence,
                    TransactionId = x.TransactionId,
                    TransactionActionId = x.TransactionActionId,
                    TransactionIdEncrypt = _dataProtectService.Encrypt(x.TransactionId.ToString()),
                    TransactionActionIdEncrypt = _dataProtectService.Encrypt(x.TransactionActionId.ToString()),
                    TransactionActionRecipientIdEncrypt = _dataProtectService.Encrypt(x.TransactionActionRecipientId.ToString()),
                    CorrespondentUserId = x.CorrespondentUserId,
                    CorrespondentUserName = culture.ToLower() == "ar" ? x.CorrespondingUserNameAR
                    : culture.ToLower() == "en" ? x.CorrespondingUserNameEn : x.CorrespondingUserNameFn,
                    DirectedToId = x.DirectedToUserId ?? x.DirectedToOrganizationId,
                    DirectedToName = x.DirectedToUserId != null ? (culture.ToLower() == "ar" ? x.ToUserFullNameAr
                    : culture.ToLower() == "en" ? x.ToUserFullNameEn : x.ToUserFullNameFn) :
                                                               (culture.ToLower() == "ar" ? x.ToOrganizationNameAr
                                                               : culture.ToLower() == "en" ? x.ToOrganizationNameEn
                                                               : x.ToOrganizationNameFn),
                    actionId = x.ActionId,
                    RequiredActionId = x.RequiredActionId,
                    TransactionNumberFormatted = x.TransactionNumberFormatted,
                    RequiredActionName = culture.ToLower() == "ar" ? x.RequiredActionNameAr
                    : culture.ToLower() == "en" ? x.RequiredActionNameEn : x.RequiredActionNameFn,
                    RecipientStatusId = x.RecipientStatusId,
                    RecipientStatusName = culture.ToLower() == "ar" ? x.RecipientStatusNameAr
                    : culture.ToLower() == "en" ? x.RecipientStatusNameEn : x.RecipientStatusNameFn,
                    IsUrgent = x.IsUrgent,
                    IsCC = x.IsCC,
                    //   transactionStatus = checkCurrentStatus(x.TransactionId),
                    DirectedFromId = x.FromUserId ?? x.FromOrganizationId,
                    DirectedFromName = (culture.ToLower() == "ar") ? ($"{x.FromUserNameAr} - {x.CreatedUserRoleOrganizationNameAr}") : ($"{x.FromUserNameEn} - {x.CreatedUserRoleOrganizationNameEn}"),
                    //DirectedFromName = (culture.ToLower(), string.IsNullOrEmpty(x.FromUserNameAr), string.IsNullOrEmpty(x.FromUserNameEn), string.IsNullOrEmpty(x.FromUserNameFn)) switch
                    //{
                    //    ("ar", false, _, _) => ($"{x.FromUserNameAr} - {x.CreatedUserRoleOrganizationNameAr}"),
                    //    ("ar", true, _, _) => x.FromOrganizationNameAr,
                    //    ("en", _, false, _) => ($"{x.FromUserNameEn} - {x.CreatedUserRoleOrganizationNameEn}"),
                    //    ("en", _, true, _) => x.FromOrganizationNameEn,
                    //    ("fn", _, _, false) => ($"{x.FromUserNameFn} - {x.CreatedUserRoleOrganizationNameFn}"),
                    //    ("fn", _, _, true) => x.FromOrganizationNameFn
                    //},//(culture.ToLower() == "ar" ? ($"{x.FromUserNameAr} - {_UnitOfWork.Repository<UserRole>().GetById(x.CreatedByUserRoleId).Organization.OrganizationNameAr}") : culture.ToLower() == "en" ? ($"{x.FromUserNameEn} - {_UnitOfWork.Repository<UserRole>().GetById(x.CreatedByUserRoleId).Organization.OrganizationNameEn}") : ($"{x.FromUserNameFn} - {_UnitOfWork.Repository<UserRole>().GetById(x.CreatedByUserRoleId).Organization.OrganizationNameFn}")) ?? (culture.ToLower() == "ar" ? x.FromOrganizationNameAr : culture.ToLower() == "en" ? x.FromOrganizationNameEn : x.FromOrganizationNameFn),
                    ConfidentialId = x.ConfidentialityLevelId,
                    IsConfidential = x.IsConfidential,
                    ConfidentialName = culture.ToLower() == "ar" ? x.ConfidentialityLevelNameAr : culture.ToLower() == "en" ? x.ConfidentialityLevelNameEn : x.ConfidentialityLevelNameFn,
                    transactionTypeId = x.transactionTypeId,
                    transactionTypeName = culture.ToLower() == "ar" ? x.transactionTypeNameAr : culture.ToLower() == "en" ? x.transactionTypeNameEn : x.transactionTypeNameFn,
                    TransactionActionRecipientId = x.TransactionActionRecipientId,
                    TransactionNumber = x.TransactionNumber,
                    secretSubject = string.IsNullOrEmpty(x.secretSubject) ? false : true,
                    Subject = string.IsNullOrEmpty(x.secretSubject) ? x.Subject : "#####",
                    SubjectEn = string.IsNullOrEmpty(x.secretSubject) ? x.SubjectEn : "#####",
                    SubjectFn = string.IsNullOrEmpty(x.secretSubject) ? x.SubjectFn : "#####",
                    Notes = x.Notes,
                    createdOn = x.createdOn,
                    LetterCount = x.LetterCount,
                    DocumentCount = x.DocumentCount,
                    PhysicalCount = x.PhysicalCount,
                    relatedTransactions = x.RelatedTransactionsCount,
                    ImportanceLevelId = x.ImportanceLevelId,
                    fromUserProfileImage = x.ProfileImage == null ? null : Convert.ToBase64String(x.ProfileImage),
                    ImportanceLevelName = culture.ToLower() == "ar" ? x.ImportanceLevelNameAr : culture.ToLower() == "en" ? x.ImportanceLevelNameEn : x.ImportanceLevelNameFn,
                    IsLate = x.IsLate == 1 ? true : false,
                    IndivnidualsCounts = x.IndivnidualsCounts,
                    ImportanceLevelColor = x.ImportanceLevelColor,
                    iswared = x.IncomingLetterDate != null || x.IncomingOrganizationId != null ? true : false,
                    incomingOrganizationName = string.IsNullOrEmpty(x.incomingOrganizationNameAr) ? "" : (culture.ToLower() == "ar" ? x.incomingOrganizationNameAr : culture.ToLower() == "en" ? x.incomingOrganizationNameEn : x.incomingOrganizationNameFn),
                    //   TransactionIdEncrypt = _dataProtectService.Encrypt(x.TransactionId.ToString()),
                    //   TransactionActionIdEncrypt = _dataProtectService.Encrypt(x.TransactionActionId.ToString()),
                    //  TransactionActionRecipientIdEncrypt = _dataProtectService.Encrypt(x.TransactionActionRecipientId.ToString()),
                    IsShowAllTime = x.IsShowAllTime,
                    transactionActionRecipientAttachment = GetTransactionActionRecipientsAttachments(0, 0, x.TransactionActionRecipientId, true).Select(t => new AttachmentViewDTO
                    {
                        Id = t.Id,
                        TransactionAttachmentId = t.TransactionAttachmentId,
                        AttachmentId = t.AttachmentId,
                        AttachmentName = t.AttachmentName,
                        AttachmentTypeId = t.AttachmentTypeId,
                        OriginalName = t.OriginalName,
                        MimeType = t.MimeType,
                        Size = t.Size,
                        Width = t.Width,
                        Height = t.Height,
                        LFEntryId = t.LFEntryId,
                        PageCount = t.PagesCount,
                        Notes = t.Notes,
                        PhysicalAttachmentTypeNameAr = t.PhysicalAttachmentTypeNameAr,
                        PhysicalAttachmentTypeNameEn = t.PhysicalAttachmentTypeNameEn,
                        AttachmentTypeCode = t.AttachmentTypeCode,
                        AttachmentTypeNameAr = t.AttachmentTypeNameAr,
                        AttachmentTypeNameEn = t.AttachmentTypeNameEn,
                        TransactionId = t.TransactionId,
                        FromRelatedTransaction = t.FromRelatedTransaction,
                        CreatedByFullNameAr = t.CreatedByFullNameAr,
                        CreatedByFullNameEn = t.CreatedByFullNameEn,
                        CreatedByFullNameFn = t.CreatedByFullNameFn,
                        OrgnazationNameAr = t.OrgnazationNameAr,
                        OrgnazationNameEn = t.OrgnazationNameEn,
                        OrgnazationNameFn = t.OrgnazationNameFn,
                        CreatedOn = t.CreatedOn
                    }),
                    IsFinished = x.IsFinished,
                    FollowUPFinishedDate = x.FollowUPFinishedDate,
                    IsPrinted = x.IsPrinted,
                    CreatedUserName = x.CreatedUserName, 
                    transactionStatus = GetTransactionStatus(x.TransactionId, culture)
                    // PreprationDetails = filterId == 12 ? _transactionService.GetPreparationDetails(x.TransactionActionId) : null,
                    // IsRejected = x.IsRejected

                });
                return new DataSourceResult<TransactionBoxDTO> { Data = inboxData, Count = countData };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // Get Transaction Status
        

        #region OutboxDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="isCount"></param>
        /// <param name="isEmployee"></param>
        /// <param name="filterId"></param>
        /// <returns>return GetOutbox Details of type TransactionBoxDTO</returns>
        private DataSourceResult<TransactionBoxDTO> GetOutboxDetails(string searchText, int page, int pageSize, bool isCount, int? CommitteId, bool isEmployee = true, int filterId = 0, InboxFilterFieldsDTo inboxFilterFields = null)
        {
            int userId = _sessionServices.UserId.Value;
            if (inboxFilterFields.SearchInSpecificUser_UserId != null)
            {
                userId = inboxFilterFields.SearchInSpecificUser_UserId.Value;
                isEmployee = true;
            }
            int organizationId = 0;
            int UserRoleId = 0;
            var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
            var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
            UserRoleId = userRole.UserRoleId;
            long countOutboxData = 0;
            IEnumerable<Vw_TransactionBoxes> transactionData = new List<Vw_TransactionBoxes>();
            if (filterId == 6 /*confirmation*/)
            {
                if (isCount)
                {
                    countOutboxData = GetOutboxConfirmationCount(page, pageSize, isCount, organizationId, userId, isEmployee, searchText, filterId, UserRoleId, inboxFilterFields);
                }
                transactionData = GetOutConfirmationbox(page, pageSize, false, organizationId, userId, isEmployee, searchText, filterId, UserRoleId, inboxFilterFields);
            }

            else
            {
                if (isCount)
                {
                    countOutboxData = GetOutboxCount(page, pageSize, isCount, organizationId, userId, isEmployee, searchText, filterId, UserRoleId, CommitteId, inboxFilterFields);
                }
                transactionData = GetOutbox(page, pageSize, false, organizationId, userId, isEmployee, searchText, filterId, UserRoleId, CommitteId, inboxFilterFields);
            }

            IEnumerable<TransactionBoxDTO> OutboxData = transactionData.OrderByDescending(x => x.Id)
                .Select(x => new TransactionBoxDTO
                {
                    TransactionId = x.TransactionId,
                    TransactionActionId = x.TransactionActionId,
                    CorrespondentUserId = x.CorrespondentUserId,
                    CorrespondentUserName = culture.ToLower() == "ar" ? x.CorrespondingUserNameAR : culture.ToLower() == "en" ? x.CorrespondingUserNameEn : x.CorrespondingUserNameFn,
                    DirectedToId = x.DirectedToUserId ?? x.DirectedToOrganizationId,
                    IsDirectedToUser = x.DirectedToUserId != null ? true : false,
                    DirectedToName = x.DirectedToUserId != null ? (culture.ToLower() == "ar" ? x.ToUserFullNameAr : culture.ToLower() == "en" ? x.ToUserFullNameEn : x.ToUserFullNameFn) :
                                                               (culture.ToLower() == "ar" ? x.ToOrganizationNameAr : culture.ToLower() == "en" ? x.ToOrganizationNameEn : x.ToOrganizationNameFn),
                    actionId = x.ActionId,
                    RequiredActionId = x.RequiredActionId,
                    RequiredActionName = culture.ToLower() == "ar" ? x.RequiredActionNameAr : culture.ToLower() == "en" ? x.RequiredActionNameEn : x.RequiredActionNameFn,
                    RecipientStatusId = x.RecipientStatusId,
                    RecipientStatusName = culture.ToLower() == "ar" ? x.RecipientStatusNameAr : culture.ToLower() == "en" ? x.RequiredActionNameEn : x.RequiredActionNameFn,
                    IsUrgent = x.IsUrgent,
                    IsCC = x.IsCC,
                    isCancelled = x.IsCancelled,
                    //      transactionStatus = checkCurrentStatus(x.TransactionId),
                    DirectedFromId = x.FromUserId ?? x.FromOrganizationId,
                    DirectedFromName = (culture.ToLower() == "ar" ? x.FromUserNameAr : culture.ToLower() == "en" ? x.FromUserNameEn : x.FromUserNameFn) ?? (culture.ToLower() == "ar" ? x.FromOrganizationNameAr : culture.ToLower() == "en" ? x.FromOrganizationNameEn : x.FromOrganizationNameFn),
                    ConfidentialId = x.ConfidentialityLevelId,
                    IsConfidential = x.IsConfidential,
                    ConfidentialName = culture.ToLower() == "ar" ? x.ConfidentialityLevelNameAr : culture.ToLower() == "en" ? x.ConfidentialityLevelNameEn : x.ConfidentialityLevelNameFn,
                    transactionTypeId = x.transactionTypeId,
                    transactionTypeName = culture.ToLower() == "ar" ? x.transactionTypeNameAr : culture.ToLower() == "en" ? x.transactionTypeNameEn : x.transactionTypeNameFn,
                    TransactionActionRecipientId = x.TransactionActionRecipientId,
                    TransactionNumber = x.TransactionNumber,
                    TransactionNumberFormatted = x.TransactionNumberFormatted,
                    secretSubject = string.IsNullOrEmpty(x.secretSubject) ? false : true,
                    Subject = string.IsNullOrEmpty(x.secretSubject) ? x.Subject : "#####",
                    SubjectEn = string.IsNullOrEmpty(x.secretSubject) ? x.SubjectEn : "#####",
                    SubjectFn = string.IsNullOrEmpty(x.secretSubject) ? x.SubjectFn : "#####",
                    archieveDate = x.archieveDate == null ? 0 : x.archieveDate,
                    createdOn = x.createdOn,
                    LetterCount = x.ActionLetterCount,
                    DocumentCount = x.ActionDocumentCount,
                    PhysicalCount = x.ActionPhysicalCount,
                    relatedTransactions = x.RelatedTransactionsCount,
                    ImportanceLevelId = x.ImportanceLevelId,
                    fromUserProfileImage = x.ProfileImage == null ? null : Convert.ToBase64String(x.ProfileImage),
                    ImportanceLevelName = culture.ToLower() == "ar" ? x.ImportanceLevelNameAr : culture.ToLower() == "en" ? x.ImportanceLevelNameEn : x.ImportanceLevelNameFn,
                    //IsLate = x.IsLate,
                    IsLate = x.IsLate == 1 ? true : false,
                    IndivnidualsCounts = x.IndivnidualsCounts,
                    ImportanceLevelColor = x.ImportanceLevelColor,
                    IsFinished = x.IsFinished,
                    FollowUPFinishedDate = x.FollowUPFinishedDate,
                    transactionStatus = GetTransactionStatus(x.TransactionId , culture),
                    //TransactionIdEncrypt = _dataProtectService.Encrypt(x.TransactionId.ToString()),
                    //TransactionActionIdEncrypt = _dataProtectService.Encrypt(x.TransactionActionId.ToString()),
                    //TransactionActionRecipientIdEncrypt = _dataProtectService.Encrypt(x.TransactionActionRecipientId.ToString()),
                    //IsShowAllTime = x.IsShowAllTime,

                    transactionActionRecipientAttachment = GetTransactionActionRecipientsAttachments(0, x.TransactionActionId, 0, true).Select(t => new AttachmentViewDTO
                    {
                        Id = t.Id,
                        TransactionAttachmentId = t.TransactionAttachmentId,
                        AttachmentId = t.AttachmentId,
                        AttachmentName = t.AttachmentName,
                        AttachmentTypeId = t.AttachmentTypeId,
                        OriginalName = t.OriginalName,
                        MimeType = t.MimeType,
                        Size = t.Size,
                        Width = t.Width,
                        Height = t.Height,
                        LFEntryId = t.LFEntryId,
                        PageCount = t.PagesCount,
                        Notes = t.Notes,
                        PhysicalAttachmentTypeNameAr = t.PhysicalAttachmentTypeNameAr,
                        PhysicalAttachmentTypeNameEn = t.PhysicalAttachmentTypeNameEn,
                        AttachmentTypeCode = t.AttachmentTypeCode,
                        AttachmentTypeNameAr = t.AttachmentTypeNameAr,
                        AttachmentTypeNameEn = t.AttachmentTypeNameEn,
                        TransactionId = t.TransactionId,
                        CreatedByFullNameAr = t.CreatedByFullNameAr,
                        CreatedByFullNameEn = t.CreatedByFullNameEn,
                        CreatedByFullNameFn = t.CreatedByFullNameFn,
                        OrgnazationNameAr = t.OrgnazationNameAr,
                        OrgnazationNameEn = t.OrgnazationNameEn,
                        OrgnazationNameFn = t.OrgnazationNameFn,
                        CreatedOn = t.CreatedOn
                    }),
                    CreatedUserName = x.CreatedUserName,
                });
            return new DataSourceResult<TransactionBoxDTO> { Data = OutboxData.Distinct(), Count = countOutboxData };
        }
        #endregion
        private string GetByKey(string key, string culture)
        {
            return (culture == "ar") ? _UnitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == key).CommiteeLocalizationAr
                   : _UnitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == key).CommiteeLocalizationEn;
        }
        private string checkCurrentStatus(long TransactionId)
        {
            TransactionActionRecipient _TransactionActionRecipient = _UnitOfWork.GetRepository<TransactionActionRecipient>().GetAll().Where(a => a.TransactionAction.Transaction.TransactionId == TransactionId && a.IsCc == false).LastOrDefault();
            string transactionCurrentStatus = "";
            if (_TransactionActionRecipient != null)
            {
                int RecipientStatusId = _TransactionActionRecipient.RecipientStatusId == null ? 0 : (int)_TransactionActionRecipient.RecipientStatusId;
                int ActionId = _TransactionActionRecipient.TransactionAction.ActionId;
                if (RecipientStatusId == (int)RecipientStatusesEnum.Sent || ActionId == (int)ActionEnum.ExternalDelegationExecute)
                {
                    transactionCurrentStatus = GetByKey("StatusSent", culture);


                }
                else if (ActionId == (int)ActionEnum.ExternalDelegationRequest)
                {
                    transactionCurrentStatus = GetByKey("StatusUnderExport", culture);


                }
                else if (RecipientStatusId == (int)RecipientStatusesEnum.Saved)
                {
                    transactionCurrentStatus = GetByKey("StatusSaved", culture);
                }

                else
                {
                    transactionCurrentStatus = GetByKey("StatusInProgress", culture);

                }
            }
            else
                transactionCurrentStatus = GetByKey("StatusInProgress", culture);

            return transactionCurrentStatus;

        }
        #region Inbox
        public IEnumerable<Vw_TransactionBoxes_Inbox> GetInbox(int page, int pageSize, bool count, int? organizationId, int? userId, bool isEmployee, string searchtxt, int filterId, int? userRoleId, InboxFilterFieldsDTo inboxFilterFields = null, bool isForAllSystem = false)
        {
            try
            {
                int? fromIncomingOrganizationId = inboxFilterFields?.FromIncomingOrganizationId;
                bool? IsEmployeeFilter = inboxFilterFields?.IsEmployeeFilter;
                int? fromId = inboxFilterFields?.FromId;
                int? Case = inboxFilterFields?.FilterCase;
                var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
                var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
                userRoleId = userRole.UserRoleId;
                IQueryable<Vw_TransactionBoxes_Inbox> inbox = this._context.Vw_TransactionBoxes_Inbox.FromSqlInterpolated($"exec sp_GetInbox {((page - 1) * pageSize)},{pageSize},{count},{organizationId},{userId},{isEmployee},{searchtxt},{filterId} ,{userRoleId},{fromIncomingOrganizationId},{inboxFilterFields?.From},{inboxFilterFields?.To},{IsEmployeeFilter},{fromId},{Case},{isForAllSystem}");
                return inbox;

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public long GetInboxCount(int page, int pageSize, bool count, int? organizationId, int? userId, bool isEmployee, string searchtxt, int filterId, int userRoleId, InboxFilterFieldsDTo inboxFilterFields = null, bool isForAllSystem = false)
        {
            int? fromIncomingOrganizationId = inboxFilterFields?.FromIncomingOrganizationId;
            bool? IsEmployeeFilter = inboxFilterFields?.IsEmployeeFilter;
            int? fromId = inboxFilterFields?.FromId;
            int? Case = inboxFilterFields?.FilterCase;
            var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
            var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
            userRoleId = userRole.UserRoleId;
            var x = this._context.COUNTS.FromSqlInterpolated($"exec sp_GetInbox {((page - 1) * pageSize)},{pageSize},{count},{organizationId},{userId},{isEmployee},{searchtxt},{filterId},{userRoleId},{fromIncomingOrganizationId},{inboxFilterFields?.From},{inboxFilterFields?.To},{IsEmployeeFilter},{fromId},{Case},{isForAllSystem}").FirstOrDefault().CNT;
            return x;
        }

        #endregion



        #region Inbox
        public IEnumerable<Vw_TransactionBoxes_Inbox> GetInbox(int page, int pageSize, bool count, int? organizationId, int? userId, bool isEmployee, string searchtxt, int filterId, int? userRoleId, int? CommitteId, InboxFilterFieldsDTo inboxFilterFields = null, bool isForAllSystem = false)
        {
            try
            {
                int? fromIncomingOrganizationId = inboxFilterFields?.FromIncomingOrganizationId;
                bool? IsEmployeeFilter = inboxFilterFields?.IsEmployeeFilter;
                int? fromId = inboxFilterFields?.FromId;
                int? Case = inboxFilterFields?.FilterCase;
                var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
                var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
                userRoleId = userRole.UserRoleId;
                IQueryable<Vw_TransactionBoxes_Inbox> inbox = this._context.Vw_TransactionBoxes_Inbox.FromSqlInterpolated($"exec sp_GetInbox {((page - 1) * pageSize)},{pageSize},{count},{organizationId},{userId},{isEmployee},{searchtxt},{filterId} ,{userRoleId},{fromIncomingOrganizationId},{inboxFilterFields?.From},{inboxFilterFields?.To},{IsEmployeeFilter},{fromId},{Case},{isForAllSystem},{CommitteId}");
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
            var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
            var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
            userRoleId = userRole.UserRoleId;
            var x = this._context.COUNTS.FromSqlInterpolated($"exec sp_GetInbox {((page - 1) * pageSize)},{pageSize},{count},{organizationId},{userId},{isEmployee},{searchtxt},{filterId},{userRoleId},{fromIncomingOrganizationId},{inboxFilterFields?.From},{inboxFilterFields?.To},{IsEmployeeFilter},{fromId},{Case},{isForAllSystem},{CommitteId}").FirstOrDefault().CNT;
            return x;
        }
        #endregion
        public IEnumerable<TransactionByTypeReportDTO> GetTransactionByTypeReport(int userId, int OrganizationId, int userRoleId, DateTimeOffset? fROM_DATE, DateTimeOffset? tO_DATE)
        {
            var from = (fROM_DATE == null) ? "" : fROM_DATE.ToString();
            var to = (tO_DATE == null) ? "" : tO_DATE.ToString();
            var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
            var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
            userRoleId = userRole.UserRoleId;
            return this._context.TransactionByTypeReportDTO.FromSqlInterpolated($"exec GetTransactionByTypeReport {userId},{OrganizationId},{userRoleId},{from},{to}");
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
                var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
                var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
                UserRoleId = userRole.UserRoleId;
                var Data = this._context.ViewTransactionsView.FromSqlInterpolated($"exec sp_GetOutboxForCommittee {((page - 1) * pageSize)},{pageSize},{count},{organizationId},{userId},{0},{searchtxt},{filterId},{UserRoleId},{fromIncomingOrganizationId},{null},{null},{true},{fromId},{Case},{0},{CommitteId}");
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
                var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
                var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
                UserRoleId = userRole.UserRoleId;

                return this._context.COUNTS.FromSqlInterpolated($"exec sp_GetOutboxForCommittee {((page - 1) * pageSize)},{pageSize},{count},{organizationId},{userId},{isEmployee},{searchtxt},{filterId},{UserRoleId},{fromIncomingOrganizationId},{null},{null},{IsEmployeeFilter},{fromId},{Case},{0},{CommitteId}").AsEnumerable().FirstOrDefault().CNT;
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
                _context.COUNTS.FromSqlInterpolated($"exec sp_UpdateMaxTransactionActionReceipientId {TransactionId},{IsEmployee},{FromOrganizationId},{FromUserId}").ToList();
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

                var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
                var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
                userRoleId = userRole.UserRoleId;
                return this._context.COUNTS.FromSqlInterpolated($"exec sp_GetOutConfirmationbox {((page - 1) * pageSize)},{pageSize},{isCount},{"''"},{userId},{isEmployee},{"''"},{"''"},{"''"},{"''"},{"''"},{"''"},{IsEmployeeFilter},{"''"},{Case}").FirstOrDefault().CNT;
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
                var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
                var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
                userRoleId = userRole.UserRoleId;
                var Data = this._context.ViewTransactionsView.FromSqlInterpolated($"exec sp_GetOutConfirmationbox {((page - 1) * pageSize)},{pageSize},{isCount},{organizationId},{userId},{isEmployee},{searchText},{filterId},{userRoleId},{fromIncomingOrganizationId},{inboxFilterFields?.From},{inboxFilterFields?.To},{IsEmployeeFilter},{fromId},{Case}");
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
                return _context.attachmentsViews.FromSqlInterpolated($"exec sp_GetTransactionActionRecipientsAttachments {transactionId},{transactionActionId},{transactionActionRecipientId},{ShowRelatedAttachments},{_sessionServices.UserId},{_sessionServices.UserRoleId}").ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IEnumerable<Lookup> GetEmployeesToReferral(bool isEmployee, bool? isFavourite, string searchText)
        {
            searchText = searchText == null ? "" : searchText;
            var RoleId = (from u in _context.Users

                          join uR in _context.UserRoles
                         on u.UserId equals uR.UserId
                          where uR.OrganizationId == _sessionServices.OrganizationId && uR.UserId == _sessionServices.UserId
                          select uR.RoleId).Distinct().FirstOrDefault();
            var emps = _context.Vw_UsersToReferral.FromSqlInterpolated($"exec [dbo].[sp_GetEmployeesToReferral] {isEmployee},{_sessionServices.OrganizationId},{_sessionServices.UserId },{RoleId/*_sessionServices.RoleId*/},{isFavourite},{searchText},{true}").ToList();
            //(_UnitOfWork.GetRepository<Transaction>())
            //    .GetEmployeesToReferral(isEmployee, _sessionServices.OrganizationId ?? 0, _sessionServices.UserId ?? 0, _sessionServices.RoleId ?? 0, isFavourite, searchText ?? "")
            //    //.Where(w => w.Id != _SessionServices.UserId)
            return emps.Select(x => new Lookup
            {
                Id = x.Id,
                Text = (_sessionServices.Culture.ToLower() == "ar" ? x.FullNameAr
                             : _sessionServices.Culture.ToLower() == "en" ? x.FullNameEn
                             : x.FullNameFn),
                Additional = new LookupAdditional { /*ImageId = x.ProfileImageFileId,*/ Data = new { OrganizationName = (_sessionServices.Culture.ToLower() == "ar" ? x.OrganizationNameAr : _sessionServices.Culture.ToLower() == "en" ? x.OrganizationNameEn : x.OrganizationNameFn), CanReceiveNotification = x.NotificationByMail || x.NotificationBySMS } }
            }).ToList();
        }

        public IEnumerable<Lookup> GetOrganizationToReferral(bool isEmployee, bool? isExternalOrg, bool? isFavourite, string searchText,int committeId , int? transactionActionRecepientId, int? organizationId)
        {
            if (transactionActionRecepientId != null)
            {
                var transactionAction = _unitOfWork.GetRepository<TransactionActionRecipient>().GetAll()
                    .FirstOrDefault(a => a.TransactionActionRecipientId == transactionActionRecepientId).TransactionAction;

                var beforePreparationTransactionActionRecepient = Find_Basic_TransactionAction_of_WorkFlowProcess(transactionAction.TransactionActionId);
                if (transactionAction.ActionId == (int)ActionEnum.Preparation || transactionAction.ActionId == (int)ActionEnum.Confirmation)
                {
                    var OrgId = beforePreparationTransactionActionRecepient.DirectedToOrganizationId ?? 0;
                    return _context.VwOrganizationsToReferrals.FromSqlInterpolated($"exec [dbo].[sp_GetOrganizationToReferral] {isEmployee},{OrgId},{null},{(int)RoleEnum.Manager},{isExternalOrg},{isFavourite},{searchText}")
                    .Select(x => new Lookup
                    {
                        Id = x.Id,
                        Text = x.Code.ToString() + " - " + (_sessionServices.Culture.ToLower() == "ar" ? x.OrganizationNameAr
                                                          : _sessionServices.Culture.ToLower() == "en" ? x.OrganizationNameEn :
                                                          ""),
                        Additional = new LookupAdditional
                        {
                            Description = _sessionServices.Culture.ToLower() == "ar" ? x.FullPathAr
                                                                        : _sessionServices.Culture.ToLower() == "en" ? x.FullPathEn : ""

                        }
                    });
                }
            }

            
            int? UserId = organizationId != null ? (_sessionServices.UserId ?? 0) : 0;
            //int? RoleId = organizationId == null ?  (int)RoleEnum.Manager : (_sessionServices.RoleId ?? 0);
            searchText = searchText == null ? " " : searchText;
            // Getting Organization by CommitteId then get roleId from Masar
           var OrgainzationId =  _unitOfWork.GetRepository<Commitee>().GetAll().Where(x => x.CommiteeId == committeId).FirstOrDefault().DepartmentLinkId;

            var RoleId = (from u in _context.Users
                         
                         join uR in _context.UserRoles
                        on u.UserId equals uR.UserId
                        where uR.OrganizationId == OrgainzationId && uR.UserId == UserId
                         select uR.RoleId).Distinct().FirstOrDefault();

            var orgs = _context.VwOrganizationsToReferrals.FromSqlInterpolated($"exec [dbo].[sp_GetOrganizationToReferral] {isEmployee},{OrgainzationId},{UserId},{RoleId},{isExternalOrg},{false},{searchText}").ToList();
            //    (_UnitOfWork.GetRepository<Transaction>() as ITransactionRepository)
            //.GetOrganizationToReferral(isEmployee, OrganizationId ?? SessionServices.OrganizationId ?? 0, UserId, RoleId, isExternalOrg, isFavourite, searchText ?? "")
            return orgs.Select(x => new Lookup
            {
                Id = x.Id,
                Text = x.Code.ToString() + " - " + (_sessionServices.Culture.ToLower() == "ar" ? x.OrganizationNameAr
                                                      : _sessionServices.Culture.ToLower() == "en" ? x.OrganizationNameEn
                                                      : ""),
                Additional = new LookupAdditional
                {
                    Description = _sessionServices.Culture.ToLower() == "ar" ? x.FullPathAr
                                                                    : _sessionServices.Culture.ToLower() == "en" ? x.FullPathEn
                                                                    : ""
                }
            });
        }
        #endregion
        public TransactionActionRecipient Find_Basic_TransactionAction_of_WorkFlowProcess(int _transactionActionId)
        {
            try
            {
                // Get Basic TransactionAction and TransactionActionRecipient to Find Valid Process
                TransactionAction transactionAction = _unitOfWork.GetRepository<TransactionAction>().GetAll()
                    .FirstOrDefault(w =>
                                w.TransactionActionId == _transactionActionId);

                /* In Case Off there is ReferrerTransactionActionId is null */
                if (transactionAction == null)
                {
                    return null;
                }

                if (transactionAction.ActionId == (int)ActionEnum.Preparation)
                {
                    /* get Basic TransactionActionRecipient from Preparation Transaction
                     * throw ReferrerTransactionActionId 
                     * And ReferrerTransactionActionId */

                    TransactionActionRecipient BasictransactionActionRecipient = _unitOfWork.GetRepository<TransactionActionRecipient>().GetAll()
                  .FirstOrDefault(w =>
                              w.TransactionActionId == transactionAction.ReferrerTransactionActionId
                              && w.TransactionActionRecipientId == transactionAction.ReferrerTransactionActionRecipientId);
                    return BasictransactionActionRecipient;
                }

                else
                {
                    return Find_Basic_TransactionAction_of_WorkFlowProcess(transactionAction.ReferrerTransactionActionId == null ? 0 : transactionAction.ReferrerTransactionActionId.Value);
                }
            }
            catch (Exception Ex)
            {

                throw;
            }

        }
        private static readonly object LockObject = 1;
        public string GenerateTransactionNumber(int TransactionTypeId, string transSerial)
        {
            string Format = _SystemSettingsService.GetSystemSettingByCode("TransactionNumberFormat").SystemSettingValue.Trim().ToUpper();
            StringBuilder sb = new StringBuilder(Format);
            string SFormat = Format;
            char[] Delimiters = new char[] { '.', '-', '_', '/', '\\', '$' };
            IOrderedEnumerable<string> Segments = Format.Split(Delimiters).OrderByDescending(x => x.Length);
            Segments.AsParallel().ForAll(x => sb.Replace(x, GetFormatSegment(x, TransactionTypeId)));
            string TransactionNumber = GetNextTransactionNumber(sb.ToString(), TransactionTypeId, transSerial);
            // To Remove Separator
            string[] DelimitersForRemove = { "$" };
            for (int i = 0; i < DelimitersForRemove.Length; i++)
            {
                TransactionNumber = TransactionNumber.Replace(DelimitersForRemove[i], "");
            }
            ///////////
            return TransactionNumber;

        }
        public string GenerateTransactionNumberFormat(int TransactionTypeId, string transSerial, string addCodeValue)
        {
            string Format = _SystemSettingsService.GetSystemSettingByCode("showTransactionNumber").SystemSettingValue.Trim().ToUpper();
            StringBuilder sb = new StringBuilder(Format);
            string SFormat = Format;
            char[] Delimiters = new char[] { '.', '-', '_', '/', '\\', '$' };
            IOrderedEnumerable<string> Segments = Format.Split(Delimiters).OrderByDescending(x => x.Length);
            Segments.AsParallel().ForAll(x => sb.Replace(x, GetFormatSegment(x, TransactionTypeId)));
            string TransactionNumber = GetNextTransactionNumberShow(sb.ToString(), TransactionTypeId, transSerial);
            // To Remove Separator
            string[] DelimitersForRemove = { "$" };
            for (int i = 0; i < DelimitersForRemove.Length; i++)
            {
                TransactionNumber = TransactionNumber.Replace(DelimitersForRemove[i], "");
            }
            ///////////
            return TransactionNumber + addCodeValue;

        }
        private string GetFormatSegment(string segment, int TransactionTypeId)
        {
            UmAlQuraCalendar hijri = new UmAlQuraCalendar();
            string ToBeReturned = string.Empty;
            switch (segment)
            {
                case "Y":
                    ToBeReturned = DateTimeOffset.Now.Year.ToString().Substring(2, segment.Length);
                    break;

                case "YY":
                    ToBeReturned = DateTimeOffset.Now.Year.ToString().Substring(2, segment.Length);
                    break;

                case "YYY":
                    ToBeReturned = DateTimeOffset.Now.Year.ToString().Substring(1, segment.Length);
                    break;

                case "YYYY":
                    ToBeReturned = DateTimeOffset.Now.Year.ToString().Substring(0, segment.Length);
                    break;

                case "H":
                    ToBeReturned = hijri.GetYear(DateTime.Now).ToString().Substring(2, segment.Length);
                    break;

                case "HH":
                    ToBeReturned = hijri.GetYear(DateTime.Now).ToString().Substring(2, segment.Length);
                    break;

                case "HHH":
                    ToBeReturned = hijri.GetYear(DateTime.Now).ToString().Substring(1, segment.Length);
                    break;

                case "HHHH":
                    ToBeReturned = hijri.GetYear(DateTime.Now).ToString().Substring(0, segment.Length);
                    break;

                case "MY":
                    ToBeReturned = DateTimeOffset.Now.Month.ToString().Length <= 1 ? "0" + DateTimeOffset.Now.Month.ToString() : DateTimeOffset.Now.Month.ToString();
                    break;

                case "MH":
                    ToBeReturned = hijri.GetMonth(DateTime.Now).ToString().Length <= 1 ? "0" + hijri.GetMonth(DateTime.Now).ToString() : hijri.GetMonth(DateTime.Now).ToString();
                    break;
                case "DY":
                    ToBeReturned = DateTimeOffset.Now.Day.ToString().Length <= 1 ? "0" + DateTimeOffset.Now.Day.ToString() : DateTimeOffset.Now.Day.ToString();
                    break;

                case "DH":
                    ToBeReturned = hijri.GetDayOfMonth(DateTime.Now).ToString().Length <= 1 ? "0" + hijri.GetDayOfMonth(DateTime.Now).ToString() : hijri.GetDayOfMonth(DateTime.Now).ToString();
                    break;

                case "RT":
                case "T":
                    ToBeReturned = base._UnitOfWork.GetRepository<TransactionType>().GetAll()
                        .Where(x => x.TransactionTypeId == TransactionTypeId)
                        .Select(x => x.TransactionTypeCodeForSerial).FirstOrDefault();
                    ToBeReturned = string.Format("{0}", ToBeReturned);
                    ToBeReturned = ToBeReturned == null ? "" : ToBeReturned;
                    break;
                case "RTT":
                case "TT":
                    ToBeReturned = base._UnitOfWork.GetRepository<TransactionType>().GetAll()
                        .Where(x => x.TransactionTypeId == TransactionTypeId)
                        .Select(x => x.TransactionTypeCodeForSerial).FirstOrDefault();
                    ToBeReturned = string.Format("{0:00}", int.Parse(ToBeReturned));
                    break;
                case "RTTT":
                case "TTT":
                    ToBeReturned = base._UnitOfWork.GetRepository<TransactionType>().GetAll()
                        .Where(x => x.TransactionTypeId == TransactionTypeId)
                        .Select(x => x.TransactionTypeCodeForSerial).FirstOrDefault();
                    ToBeReturned = string.Format("{0:000}", int.Parse(ToBeReturned));
                    break;
                case "OG":
                case "G":
                    ToBeReturned = base._UnitOfWork.GetRepository<Organization>().GetAll()
                       .FirstOrDefault(x => x.OrganizationId == _sessionServices.OrganizationId.Value)
                       .Code.ToString();
                    ToBeReturned = string.Format("{0}", ToBeReturned);
                    break;
            }
            return ToBeReturned;
        }

        private string GetNextTransactionNumberShow(string Format, int TransactionTypeId, string transSerial)
        {
            string oldFormat = _SystemSettingsService.GetSystemSettingByCode("showTransactionNumber").SystemSettingValue.Trim().ToUpper();
            StringBuilder sb = new StringBuilder(Format);
            //  _IGenericRepository<Transaction> TransRepository = base._UnitOfWork.Repository<Transaction>();
            HijriCalendar hijri = new HijriCalendar();
            int FirstIndexOfS = oldFormat.ToUpper().IndexOf("S");
            int LastIndexOfS = oldFormat.ToUpper().LastIndexOf("S");
            int SerialLength = (LastIndexOfS - FirstIndexOfS) + 1;
            string NextSerial = string.Empty;
            NextSerial = transSerial;// transactionRepository.GetNextTransactionNumberSerial(TransactionTypeId).ToString();
            NextSerial = GetCorrectLengthOfSerial(NextSerial, SerialLength);
            Format += NextSerial;
            // return Format;
            return Format;
        }
        private string GetNextTransactionNumber(string Format, int TransactionTypeId, string transSerial)
        {
            string oldFormat = _SystemSettingsService.GetSystemSettingByCode("TransactionNumberFormat").SystemSettingValue.Trim().ToUpper();
            StringBuilder sb = new StringBuilder(Format);
            //  _IGenericRepository<Transaction> TransRepository = base._UnitOfWork.GetRepository<Transaction>();
            HijriCalendar hijri = new HijriCalendar();
            int FirstIndexOfS = oldFormat.ToUpper().IndexOf("S");
            int LastIndexOfS = oldFormat.ToUpper().LastIndexOf("S");
            int SerialLength = (LastIndexOfS - FirstIndexOfS) + 1;
            string NextSerial = string.Empty;

            #region Old
            //string SerialPattern = string.Empty;
            // Transaction LastTransaction =new Transaction();
            //for (int i = 0; i < SerialLength; i++)
            //{
            //    SerialPattern += "S";
            //}

            //Format += SerialPattern;

            //  Transaction LastTransaction;
            //int count = 0;
            //string AutoResetTransctionNumberPattern = this._SystemSettingsService.GetSystemSettingByCode("AutoResetTransctionNumberPattern").SystemSettingValue;
            //if (oldFormat.ToUpper().Contains("RT"))
            //{
            //    LastTransaction = base._UnitOfWork.Repository<Transaction>().GetAll().Where(x => x.TransactionTypeId == TransactionTypeId && x.TransactionNumber != null
            //     && ((AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.Daily).ToString() && LastTransaction.CreatedOn.Value.Day == DateTimeOffset.Now.Day)
            //                    || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.MonthlyGregorian).ToString() && LastTransaction.CreatedOn.Value.Month == DateTimeOffset.Now.Month)
            //                    || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.MonthlyHijri).ToString() && LastTransaction.CreatedOn.Value.Month == DateTimeOffset.Now.Month)
            //                    || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.AnnualyGregorian).ToString() && LastTransaction.CreatedOn.Value.Year == DateTimeOffset.Now.Year)
            //                    || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.AnnualyHijri).ToString() && LastTransaction.CreatedOn.Value.Year == DateTimeOffset.Now.Year)
            //                    )
            //    ).OrderByDescending(x => x.CreatedOn).FirstOrDefault();

            //    count = base._UnitOfWork.Repository<Transaction>().GetAll().Where(x => x.TransactionTypeId == TransactionTypeId && x.TransactionNumber != null
            //                    && ((AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.Daily).ToString() && LastTransaction.CreatedOn.Value.Day == DateTimeOffset.Now.Day)
            //                    || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.MonthlyGregorian).ToString() && LastTransaction.CreatedOn.Value.Month == DateTimeOffset.Now.Month)
            //                    || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.MonthlyHijri).ToString() && LastTransaction.CreatedOn.Value.Month == DateTimeOffset.Now.Month)
            //                    || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.AnnualyGregorian).ToString() && LastTransaction.CreatedOn.Value.Year == DateTimeOffset.Now.Year)
            //                    || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.AnnualyHijri).ToString() && LastTransaction.CreatedOn.Value.Year == DateTimeOffset.Now.Year)
            //                    )
            //                    ).Count();
            //}
            //else
            //{
            //    LastTransaction = base._UnitOfWork.Repository<Transaction>().GetAll().Where(x =>  x.TransactionNumber != null
            //     && ((AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.Daily).ToString() && LastTransaction.CreatedOn.Value.Day == DateTimeOffset.Now.Day)
            //                    || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.MonthlyGregorian).ToString() && LastTransaction.CreatedOn.Value.Month == DateTimeOffset.Now.Month)
            //                    || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.MonthlyHijri).ToString() && LastTransaction.CreatedOn.Value.Month == DateTimeOffset.Now.Month)
            //                    || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.AnnualyGregorian).ToString() && LastTransaction.CreatedOn.Value.Year == DateTimeOffset.Now.Year)
            //                    || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.AnnualyHijri).ToString() && LastTransaction.CreatedOn.Value.Year == DateTimeOffset.Now.Year)
            //                    )
            //    ).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            //    //LastTransaction = base._UnitOfWork.Repository<Transaction>().GetAll().Where(x => x.TransactionNumber != null).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            //    count = base._UnitOfWork.Repository<Transaction>().GetAll().Where(x => x.TransactionNumber != null
            //                && ((AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.Daily).ToString() && LastTransaction.CreatedOn.Value.Day == DateTimeOffset.Now.Day)
            //                || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.MonthlyGregorian).ToString() && LastTransaction.CreatedOn.Value.Month == DateTimeOffset.Now.Month)
            //                || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.MonthlyHijri).ToString() && LastTransaction.CreatedOn.Value.Month == DateTimeOffset.Now.Month)
            //                || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.AnnualyGregorian).ToString() && LastTransaction.CreatedOn.Value.Year == DateTimeOffset.Now.Year)
            //                || (AutoResetTransctionNumberPattern == ((int)AutoResetTransctionNumberPatternEnum.AnnualyHijri).ToString() && LastTransaction.CreatedOn.Value.Year == DateTimeOffset.Now.Year)
            //                )
            //                ).Count();
            //}
            //var LastCreationDate = LastTransaction.CreatedOn;
            // DateTime LastCreationDate = DateTime.Now;



            //switch (Enum.Parse(typeof(AutoResetTransctionNumberPatternEnum), AutoResetTransctionNumberPattern))
            //{
            //    case AutoResetTransctionNumberPatternEnum.Daily:
            //        if (DateTimeOffset.Now.Day > LastCreationDate.Day)
            //            NextSerial = "1";
            //        else
            //        {
            //            NextSerial = (count + 1).ToString();
            //        }

            //        break;

            //    case AutoResetTransctionNumberPatternEnum.AnnualyGregorian:
            //        if (DateTimeOffset.Now.Year > LastCreationDate.Year)
            //            NextSerial = "1";
            //        else
            //        {
            //            NextSerial = (count + 1).ToString();
            //        }
            //        break;

            //    case AutoResetTransctionNumberPatternEnum.AnnualyHijri:
            //        if (hijri.GetYear(DateTime.Now) > hijri.GetYear((DateTime.Parse(LastCreationDate.ToString()))))
            //            NextSerial = "1";
            //        else
            //        {
            //            NextSerial = (count + 1).ToString();
            //        }
            //        break;

            //    case AutoResetTransctionNumberPatternEnum.MonthlyGregorian:
            //        if (DateTimeOffset.Now.Month > LastCreationDate.Month)
            //            NextSerial = "1";
            //        else
            //        {
            //            NextSerial = (count + 1).ToString();
            //        }
            //        break;
            //    case AutoResetTransctionNumberPatternEnum.MonthlyHijri:
            //        if (hijri.GetMonth(DateTime.Now) > hijri.GetMonth((DateTime.Parse(LastCreationDate.ToString()))))
            //            NextSerial = "1";
            //        else
            //        {
            //            NextSerial = (count + 1).ToString();
            //        }
            //        break;

            //    default:
            //        NextSerial = "1";
            //        break;
            //}
            #endregion
            NextSerial = transSerial;// transactionRepository.GetNextTransactionNumberSerial(TransactionTypeId).ToString();
            NextSerial = GetCorrectLengthOfSerial(NextSerial, SerialLength);
            Format += NextSerial;
            // return Format;
            return Format;
        }
        private string GetCorrectLengthOfSerial(string Serial, int Length)
        {
            while (Serial.Length < Length)
            {
                Serial = "0" + Serial;
            }
            return Serial;
        }
        /// <summary>
        /// Remove Delimiters from Transaction Number Formatted
        /// </summary>
        /// <param name="formattedString"></param>
        /// <returns></returns>
        public string removeDelimiters(string formattedString)
        {
            //string oldFormat = this._SystemSettingsService.GetSystemSettingByCode("TransactionNumberFormat").SystemSettingValue.Trim().ToUpper();
            //StringBuilder unformatted = new StringBuilder();
            //char[] Delimiters = new char[] { '.', '-', '_', '/', '\\' };
            //for (int i = 0; i < oldFormat.Length; i++)
            //{
            //    for (int j = 0; j < Delimiters.Length; j++)
            //    {
            //        if (oldFormat[i] == Delimiters[j])
            //        {
            //            formattedString = formattedString.Replace(Delimiters[j], ' ');
            //        }
            //    }
            //}
            //string[] splittedStr = formattedString.Split(' ');
            //for (int i = 0; i < splittedStr.Length; i++)
            //{
            //    unformatted.Append(splittedStr[i]);
            //}
            string[] Delimiters = { ".", "-", "_", "/", "\\", " " };
            for (int i = 0; i < Delimiters.Length; i++)
            {
                formattedString = formattedString.Replace(Delimiters[i], "");
            }
            return formattedString;
        }
        public TransactionDetailsDTO SaveTransaction(TransactionDetailsDTO transactionDetailsDTO)
        {
            Stopwatch stopwatch = new Stopwatch();
            // Begin timing.
            stopwatch.Start();
        retrySave:
            lock (LockObject)
            {
                try
                {
                    Transaction transactionObj = new Transaction();

                    if ((bool)transactionDetailsDTO.IsTransaction)
                    {
                        string Format = _SystemSettingsService.GetSystemSettingByCode("CreateTransactionNumberWhileRegistering").SystemSettingValue.Trim().ToUpper();
                        if (Format == "TRUE")
                        {
                            string addCodeValue = "";
                            int TransactionTypeId = transactionDetailsDTO.TransactionTypeId;
                            bool AddOrgCodeToSerial = _UnitOfWork.GetRepository<TransactionType>().GetAll().Where(w => w.TransactionTypeId == TransactionTypeId).FirstOrDefault().AddOrgCodeToSerial;
                            if (AddOrgCodeToSerial)
                            {
                                string addCharacterFormatForSerial = _UnitOfWork.GetRepository<SystemSetting>().GetAll().FirstOrDefault(w => w.SystemSettingCode == "addCharacterFormatForSerial")?.SystemSettingValue;
                                string OrganizationCode = _UnitOfWork.GetRepository<Organization>().GetAll().Where(w => w.OrganizationId == _sessionServices.OrganizationId).FirstOrDefault().Code.ToString();
                                addCodeValue = addCharacterFormatForSerial + OrganizationCode;
                            }
                            //   var x = _context.COUNTS.FromSqlInterpolated($"exec sp_GetNextTransactionNumberSerial {TransactionTypeId},{_sessionServices.OrganizationId}").FirstOrDefault();
                            Random rd = new Random();

                            int rand_num = rd.Next(0, int.MaxValue);
                            string TransactionNumberSerial = rand_num.ToString();
                            string transactionNumber = GenerateTransactionNumber(transactionDetailsDTO.TransactionTypeId, TransactionNumberSerial);
                            transactionObj.TransactionNumberFormatted = GenerateTransactionNumberFormat(transactionDetailsDTO.TransactionTypeId, TransactionNumberSerial, addCodeValue);
                            transactionObj.TransactionNumber = removeDelimiters(transactionNumber);
                        }
                    }


                    transactionObj.TransactionDate = transactionDetailsDTO.TransactionDate == null ? DateTimeOffset.Now : transactionDetailsDTO.TransactionDate;
                    //transaction.IncomingOutgoingLetterDate = DateTimeOffset.Now;
                    transactionObj.CreatedOn = DateTimeOffset.Now;
                    transactionObj.CreatedBy = _sessionServices.UserId;
                    transactionObj.TransactionTypeId = transactionDetailsDTO.TransactionTypeId;
                    transactionObj.Subject = transactionDetailsDTO.Subject;
                    transactionObj.NormalizedSubject = transactionDetailsDTO.NormalizedSubject;
                    transactionObj.Notes = transactionDetailsDTO.Notes;
                    transactionObj.TransactionBasisTypeId = transactionDetailsDTO.TransactionBasisTypeId;
                    transactionObj.ClassificationId = transactionDetailsDTO.ClassificationId;
                    transactionObj.ImportanceLevelId = transactionDetailsDTO.ImportanceLevelId;
                    transactionObj.ConfidentialityLevelId = transactionDetailsDTO.ConfidentialityLevelId;
                    transactionObj.IncomingTypeId = transactionDetailsDTO.IncomingTypeId;
                    transactionObj.IncomingLetterNumber = transactionDetailsDTO.IncomingLetterNumber;
                    transactionObj.IncomingLetterDate = transactionDetailsDTO.IncomingLetterDate;
                    transactionObj.IncomingOrganizationId = transactionDetailsDTO.IncomingOrganizationId;
                    transactionObj.IncomingCorrespondentName = transactionDetailsDTO.IncomingCorrespondentName;
                    transactionObj.IncomingCorrespondentMobileNumber = transactionDetailsDTO.IncomingCorrespondentMobileNumber;
                    transactionObj.IncomingCorrespondentEmail = transactionDetailsDTO.IncomingCorrespondentEmail;
                    transactionObj.ConcurrencyStamp = transactionDetailsDTO.ConcurrencyStamp;
                    transactionObj.ReferenceNumber = transactionDetailsDTO.ReferenceNumber;
                    transactionObj.ExecutionDate = transactionDetailsDTO.ExecutionDate;
                    transactionObj.ExecutionPeriod = transactionDetailsDTO.ExecutionPeriod;
                    transactionObj.IsForAll = transactionDetailsDTO.IsForAll;
                    if (transactionDetailsDTO.CompanyId.HasValue)
                        transactionObj.CompanyId = transactionDetailsDTO.CompanyId.Value;
                    //For Insert Hijri year and month
                    CultureInfo HijriCI = new CultureInfo("ar-SA");
                    transactionObj.HijriYear = int.Parse(DateTime.Now.ToString("yyyy", HijriCI));
                    transactionObj.HijriMonth = int.Parse(DateTime.Now.ToString("MM", HijriCI));
                    var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
                    var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
                    transactionObj.CreatedByUserRoleId = userRole.UserRoleId;
                    transactionObj.SubjectEn = transactionDetailsDTO.SubjectEn;
                    transactionObj.SubjectFn = transactionDetailsDTO.SubjectFn;

                    _unitOfWork.GetRepository<Transaction>().Insert(transactionObj);

                    stopwatch.Stop();

                    //    #region audit record action
                    //if (IsAuditEnabled)
                    //{

                    //    string entityCode = Enum.GetName(typeof(AuditEntities), AuditEntities.Transaction);
                    //    string actionCode = Enum.GetName(typeof(AuditActions), AuditActions.TransactionSave);

                    //    AuditService auditService = new AuditService(this._appSettings, this._SessionServices);
                    //    try
                    //    {
                    //        auditService.SendAuditRequest(null, t, entityCode, actionCode);
                    //    }
                    //    catch (Exception ex)
                    //    {


                    //    }


                    //}
                    //#endregion


                    // Update MigrationTransaction If Value isOldTransaction == true
                    if (transactionDetailsDTO.isOldTransaction && transactionDetailsDTO.migrationTransactionId.HasValue)
                    {
                        MigratedTransaction UpdateMigrationTransaction = _UnitOfWork.GetRepository<MigratedTransaction>().GetById(transactionDetailsDTO.migrationTransactionId.Value);
                        UpdateMigrationTransaction.IsTransfered = true;
                        _UnitOfWork.GetRepository<MigratedTransaction>().Update(UpdateMigrationTransaction);
                    }
                    //if (transactionDetailsDTO.isFromFax)
                    //{
                    //    TransfaredFax TransactionFax = new TransfaredFax
                    //    {
                    //        FaxId = transactionDetailsDTO.migrationTransactionId.ToString(),
                    //        TransactionId = transactionObj.TransactionId,
                    //    };
                    //    //MigratedTransaction UpdateMigrationTransaction = _UnitOfWork.Repository<MigratedTransaction>().GetById(transactionDetailsDTO.migrationTransactionId.Value);
                    //    //UpdateMigrationTransaction.IsTransfered = true;
                    //    _UnitOfWork.GetRepository<TransfaredFax>().Insert(TransactionFax);
                    //}
                    // add tags to transaction
                    if (transactionDetailsDTO.tagListIds != null && transactionDetailsDTO.tagListIds.Any())
                    {
                        foreach (var Id in transactionDetailsDTO.tagListIds)
                        {
                            while (!_unitOfWork.IsExisted(new CheckUniqueDTO { TableName = "TransactionTags", Fields = new string[] { "TransactionId", "TagId" }, Values = new string[] { transactionObj.TransactionId.ToString(), Id.ToString() } }))
                            {
                                _unitOfWork.GetRepository<TransactionTag>().Insert(new TransactionTag()
                                {
                                    TransactionId = transactionObj.TransactionId,
                                    TagId = Id,
                                    CreatedOn = DateTimeOffset.Now,
                                    CreatedBy = _sessionServices.UserId
                                });
                            }
                        }
                        _unitOfWork.SaveChanges(true);
                    }

                    return _Mapper.Map(transactionObj, transactionDetailsDTO);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.ToString().ToLower().Contains("insert duplicate key"))
                    {
                        if (stopwatch.Elapsed.TotalSeconds <= 10)
                        {
                            goto retrySave;
                        }
                        else
                        {
                            stopwatch.Stop();
                        }
                    }
                    throw ex;
                }
            }
        }

        public IEnumerable<TransactionAttachmentDTO> InsertAttachments(List<TransactionAttachmentDTO> transaction)
        {
            


            var TransactionAttachment = transaction.AsQueryable().ProjectTo<TransactionAttachment>(_Mapper.ConfigurationProvider, _sessionServices).ToList();
            var ToBereturned = _unitOfWork.GetRepository<TransactionAttachment>().Insert(TransactionAttachment);
            return _Mapper.Map(ToBereturned, typeof(IEnumerable<TransactionAttachment>), typeof(IEnumerable<TransactionAttachmentDTO>)) as IEnumerable<TransactionAttachmentDTO>;
        }
        public bool ValidateDelegation(int transactionActionRecipientId)
        {
            TransactionActionRecipient recipient = _context.TransactionActionRecipients
                             .FirstOrDefault(w => w.TransactionActionRecipientId == transactionActionRecipientId);

            RecipientStatusDTO Data = new RecipientStatusDTO { RecipientStatusId = recipient.RecipientStatusId, ActioId = recipient.TransactionAction.ActionId };
            if (Data == null)
            {
                return false;
            }
            else
            {
                // apply Business 
                if (Data.RecipientStatusId == (int)RecipientStatusesEnum.Sent || Data.RecipientStatusId == (int)RecipientStatusesEnum.Saved)
                {
                    if (!(Data.ActioId == (int)ActionEnum.Preparation))
                    {
                        return false;
                    }
                    else
                    {
                        return true;

                    }
                }
                else
                {
                    return true;

                }
            }
        }
        public bool SendNotification(Transaction transaction, List<TransactionActionRecipient> _TransactionactionRecipients, long _transactionId, int? _ReferrerTransactionActionId, int _ActionId, string _transactionNumber, string _ActionNumber, bool acceptPreviousTRAR, int? loggedInUser, int? loggedinOrganization, bool? IsTransaction)
        {

            int maxNotificationIndex = _UnitOfWork.GetRepository<Notification>().GetAll().Max(x => x.NotificationId);
            List<Notification> notification_lst;

            //في حالة احالة الى التصدير 
            if (_ActionId == (int)ActionEnum.ExternalDelegationRequest)
            {
                return true;
            }

            //في حالة قبول احالة التصدير 
            else if (acceptPreviousTRAR == true && _ActionId == (int)ActionEnum.ExternalDelegationExecute)
            {
                List<TransactionAction> trancationaction = _UnitOfWork.GetRepository<TransactionAction>()
                                      .GetAll()
                                      .Where(w => w.TransactionActionId == _ReferrerTransactionActionId).ToList();
                notification_lst = trancationaction.Select(x => new Notification
                {
                    NotificationId = maxNotificationIndex++,
                    TransactionId = _transactionId,
                    TransactionActionId = x.TransactionActionId,
                    //TransactionActionRecipientId = x.TransactionActionRecipientId,
                    NotificationTypeId = (int)NotificationTypeEnum.System,
                    UserId = x.DirectedFromUserId,
                    OrganizationId = x.DirectedFromOrganizationId,
                    CreatedBy = _sessionServices.UserId,
                    CreatedOn = DateTimeOffset.Now,
                    ContentAr = getContent(true, _ActionId, _transactionNumber, _ActionNumber),
                    ContentEn = getContent(false, _ActionId, _transactionNumber, _ActionNumber),

                }).ToList();
            }
            //in Case Of Assignments التكليفات 
            else if (IsTransaction == false)
            {
                notification_lst = _TransactionactionRecipients.Select(x => new Notification
                {

                    NotificationId = maxNotificationIndex++,
                    NotificationTypeId = (int)NotificationTypeEnum.System,
                    UserId = x.DirectedToUserId,
                    OrganizationId = x.DirectedToOrganizationId,
                    CreatedBy = _sessionServices.UserId,
                    CreatedOn = DateTimeOffset.Now,
                    ContentAr = getContent(true, _ActionId, _transactionNumber, _ActionNumber),
                    ContentEn = getContent(false, _ActionId, _transactionNumber, _ActionNumber),

                }).ToList();
            }

            //في حالة المعاملات العادية او رفض احالة التصدير او حالات السحب
            else
            {
                notification_lst = _TransactionactionRecipients.Select(x => new Notification
                {

                    NotificationId = maxNotificationIndex++,
                    TransactionId = _transactionId,
                    TransactionActionId = x.TransactionActionId,
                    TransactionActionRecipientId = x.TransactionActionRecipientId,
                    NotificationTypeId = (int)NotificationTypeEnum.System,
                    UserId = x.DirectedToUserId,
                    OrganizationId = x.DirectedToOrganizationId,
                    CreatedBy = _sessionServices.UserId,
                    CreatedOn = DateTimeOffset.Now,
                    ContentAr = getContent(true, _ActionId, _transactionNumber, _ActionNumber),
                    ContentEn = getContent(false, _ActionId, _transactionNumber, _ActionNumber),

                }).ToList();

            }

            try
            {
                _context.BulkInsert(notification_lst, new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true, UseTempDB = false });
                //SendEmail(transaction, _TransactionactionRecipients, loggedInUser, loggedinOrganization);
                _context.SaveChanges();



                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        private string getContent(bool _isArabic, int _actionId, string _transactionNumber, string _ActionNumber)
        {
            StringBuilder Content = new StringBuilder("");
            if (_actionId == (int)ActionEnum.Assignments)
            {
                Content = _isArabic ? Content.Append(" تم إاضافة تكليف جديد لديكم رقم  ").Append(_ActionNumber) :
                            Content.Append("You Have a New Assignment Number ").Append(_ActionNumber);
            }
            else if (_actionId == (int)ActionEnum.Announcement)
            {
                Content = _isArabic ? Content.Append(" تم إاضافة تنبيه جديد لديكم   ").Append(_ActionNumber) :
                            Content.Append("You Have a New Announcement ").Append(_ActionNumber);
            }
            else if (_actionId == (int)ActionEnum.ExternalDelegationExecute)
            {
                Content = _isArabic ? Content.Append(" تم تصدير المعاملة رقم  ").Append(_transactionNumber).Append(" بنجاح ") :
                            Content.Append(" Your Transaction Number ").Append(_transactionNumber).Append(" is Execute Successfully");
            }
            else if (_actionId == (int)ActionEnum.WithdrawRequest)
            {
                Content = _isArabic ? Content.Append(" لديكم طلب سحب جديد رقم  ").Append(_ActionNumber) :
                            Content.Append("You Have a New WithDraw Request Number ").Append(_ActionNumber);
            }
            else
            {
                Content = _isArabic ? Content.Append(" تم استلام معاملة  جديدة رقم  ").Append(_transactionNumber) :
                            Content.Append("You Recive a New Transaction Number").Append(_transactionNumber);
            }
            return Content.ToString();
        }
        public bool Delegate(DelegationDTO delegationDTO, IFollowUpService FollowUpService = null)
        {
            int? commitId;
            commitId= _SessionServices.UserIdAndRoleIdAfterDecrypt(delegationDTO.CommitteId,true).Id;


            //   throw new Exception("Hello there");
            string sysURL = _UnitOfWork.GetRepository<SystemSetting>().GetAll().FirstOrDefault(w => w.SystemSettingCode == "transactionActionRecipientToBeReplaced")?.SystemSettingValue;
            string url = delegationDTO.EmailDetailsUrlsDTO.detailsUrl + sysURL;
            // validate iF transaction Delegated From Inbox
            if (delegationDTO.ReferrerTransactionActionRecipientId != null)
            {
                bool validate = ValidateDelegation(delegationDTO.ReferrerTransactionActionRecipientId.Value);
                if (validate == false)
                {
                    return false;
                }

            }
            // check draft Is delegated before
            if (delegationDTO.ReferrerTransactionActionRecipientId == null
                && delegationDTO.ReferrerTransactionActionId == null
                && delegationDTO.TransactionActionId == 0
                && _UnitOfWork.GetRepository<TransactionAction>().GetAll().Where(a => a.TransactionId == delegationDTO.TransactionId).Count() > 0
                )
            {
                return false;
            }
            // check  Is has withdraw request before
            if (_UnitOfWork.GetRepository<TransactionAction>().GetAll().Where(a => a.TransactionId == delegationDTO.TransactionId
                                                                               && a.ActionId == 5 && !a.TransactionActionRecipients.FirstOrDefault().RecipientStatusId.HasValue).Count() > 0
                )
            {
                return false;
            }
            int? loggedInUser, loggedinOrganization;

            if (delegationDTO.isEmployee)
            {
                loggedInUser = _sessionServices.UserId.Value;
                loggedinOrganization = null;
            }
            else
            {
                loggedInUser = null;
                loggedinOrganization = _sessionServices.OrganizationId ?? null;
            }
            // Check If transaction Is Only Accepted  When it Comes From Inbox 
            if (delegationDTO.ReferrerTransactionActionRecipientId != null)
            {
                TransactionActionRecipient recipient = _UnitOfWork.GetRepository<TransactionActionRecipient>().GetById(delegationDTO.ReferrerTransactionActionRecipientId);
                if (recipient == null || recipient.RecipientStatusId != (int)RecipientStatusesEnum.Received && recipient.RecipientStatusId != (int)RecipientStatusesEnum.Seen && recipient.RecipientStatusId != (int)RecipientStatusesEnum.UnderPreparation)
                {
                    //recipient.IsNoteHidden = delegationDTO.IsNoteHidden;
                    if (delegationDTO.ActionId != (int)ActionEnum.Preparation && recipient.TransactionAction.ActionId != (int)ActionEnum.Preparation && recipient.RecipientStatusId != (int)RecipientStatusesEnum.Sent)
                    {
                        if (!(recipient.RecipientStatusId == (int)RecipientStatusesEnum.underConfirmation) && !(recipient.RecipientStatusId == (int)RecipientStatusesEnum.Rejected && delegationDTO.reject_fromPreparation == true))
                        {
                            return false;
                        }
                    }
                }
            }



            IList<TransactionAction> transactionActionLst = new List<TransactionAction>();
            List<TransactionActionRecipient> transactionActionRecipientLst = new List<TransactionActionRecipient>();
            List<TransactionActionRecipientAttachment> subSubTransactionActionRecipientAttachList = new List<TransactionActionRecipientAttachment>();

            TransactionActionRecipient transactionActionRecipient = new TransactionActionRecipient();
            TransactionActionAttachment transactionActionAttachment = new TransactionActionAttachment();

            List<TransactionActionAttachment> transactionActionAttachmentList = new List<TransactionActionAttachment>();

            //List<string> transactionActionRecipientsIds = new List<string>();

            //var DirectedFromOrganizationInCaseOfIgnoreWorkFlow = 0;
            //if (delegationDTO.IgnoreWorkFlowProcess)
            //{
            //    TransactionAction referral_tr_action = _transactionAction.GetAll().FirstOrDefault(w => w.TransactionActionId == delegationDTO.ReferrerTransactionActionId.Value);
            //    var first_receipient_before_preparation = _transactionAction.GetAll().OrderBy(t => t.CreatedOn).FirstOrDefault(t => t.TransactionId == delegationDTO.TransactionId && t.ActionId == (int)ActionEnum.Preparation).ReferrerTransactionActionRecipient;
            //    DirectedFromOrganizationInCaseOfIgnoreWorkFlow = first_receipient_before_preparation.DirectedToOrganizationId.Value;
            //}
            var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
            var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
            //TransactionAction hole object
            TransactionAction transactionActionObj = new TransactionAction
            {
                // TransactionActionId = delegationDTO.TransactionActionId,
                TransactionId = delegationDTO.TransactionId,
                ActionId = delegationDTO.ActionId,
                ActionNumber = delegationDTO.ActionNumber,
                ReferrerTransactionActionId = delegationDTO.ReferrerTransactionActionId,
                ReferrerTransactionActionRecipientId = delegationDTO.ReferrerTransactionActionRecipientId,
                DirectedFromUserId = loggedInUser,
                DirectedFromOrganizationId = delegationDTO.DirectedFromOrganizationId ?? loggedinOrganization, // delegationDTO.IgnoreWorkFlowProcess ? DirectedFromOrganizationInCaseOfIgnoreWorkFlow : delegationDTO.DirectedFromOrganizationId ?? loggedinOrganization,
                CreatedBy = _sessionServices.UserId.Value,
                CreatedByUserRoleId = userRole.UserRoleId,
                OutgoingTransactionNumber = delegationDTO.OutgoingTransactionNumber,
                OutgoingTransactionTypeId = delegationDTO.OutgoingTransactionTypeId,
                OutgoingImportanceLevelId = delegationDTO.OutgoingImportanceLevelId,
                OutgoingIsConfidential = delegationDTO.OutgoingIsConfidential,
                CreatedOn = DateTimeOffset.Now,
                ArchieveDate = Convert.ToInt32(DateTimeOffset.Now.Year + string.Format("{0:MM}", DateTimeOffset.Now)),
                FromCommitteId = commitId,
                //List of TransactionActionRecipients
                TransactionActionRecipients = delegationDTO.transactionActionRecipientsDTO == null
                    ? new List<TransactionActionRecipient>()
                    : delegationDTO.transactionActionRecipientsDTO.Select(transactionActionRecipientDTo => new TransactionActionRecipient
                    {
                        DirectedToUser = transactionActionRecipientDTo.DirectedToUserId != null ? _UnitOfWork.GetRepository<User>().GetAll().FirstOrDefault(u => u.UserId == transactionActionRecipientDTo.DirectedToUserId) : new User { },
                        IsUrgent = transactionActionRecipientDTo.IsUrgent,
                        SendNotification = transactionActionRecipientDTo.SendNotification,
                        CreatedBy = _sessionServices.UserId.Value,
                        Notes = transactionActionRecipientDTo.Notes,
                        NotesFn = transactionActionRecipientDTo.NotesFn,
                        NotesEn = transactionActionRecipientDTo.NotesEn,
                        TransactionActionId = transactionActionRecipientDTo.TransactionActionId,
                        DirectedToUserId = transactionActionRecipientDTo.DirectedToUserId,
                        DirectedToOrganizationId = transactionActionRecipientDTo.DirectedToOrganizationId,
                        RequiredActionId = transactionActionRecipientDTo.RequiredActionId,
                        IsCc = transactionActionRecipientDTo.IsCC,
                        CreatedOn = DateTimeOffset.Now,
                        CorrespondentUserId = transactionActionRecipientDTo.CorrespondentUserId,
                        TransactionActionRecipientId = transactionActionRecipientDTo.TransactionActionRecipientId,
                        RecipientStatusId = transactionActionRecipientDTo.RecipientStatusId,
                        UrgencyDaysCount = transactionActionRecipientDTo.UrgencyDaysCount,
                        RecipientStatusChangedBy = transactionActionRecipientDTo.RecipientStatusChangedBy,
                        RecipientStatusChangedOn = transactionActionRecipientDTo.RecipientStatusChangedOn,
                        IsHidden = transactionActionRecipientDTo.IsHidden,
                        IsNoteHidden = transactionActionRecipientDTo.IsNoteHidden,
                        //List of TransactionActionRecipientAttachments
                        TransactionActionRecipientAttachments = transactionActionRecipientDTo.transactionActionRecipientAttachmentDTO == null ? new List<TransactionActionRecipientAttachment>() : transactionActionRecipientDTo.transactionActionRecipientAttachmentDTO.Select(transactionActionRecipientAttachmentDTO => new TransactionActionRecipientAttachment
                        {
                            TransactionActionRecipientId = transactionActionRecipientDTo.TransactionActionRecipientId,
                            TransactionAttachmentId = transactionActionRecipientAttachmentDTO.TransactionAttachmentId,
                            CreatedOn = DateTimeOffset.Now,
                        }).ToList()
                    }).ToList(),

                //List of TransactionActionAttachment
                TransactionActionAttachments = delegationDTO.transactionActionAttachmentDTO == null ? new List<TransactionActionAttachment>() : delegationDTO.transactionActionAttachmentDTO.Select(transactionActionAttachmentDTO => new TransactionActionAttachment()
                {
                    TransactionActionId = delegationDTO.TransactionActionId,
                    TransactionAttachmentId = transactionActionAttachmentDTO.TransactionAttachmentId
                }).ToList(),

            };

            // --  UpdateAnnotation_IsDelegated By AnnotationId
            delegationDTO.TransactionActionAttachmentAnnotations?.ForEach(s =>
            {
                var annotation = _context.Annotations.FirstOrDefault(w => w.AnnotationId == s.AnnotationId);
                if (annotation == null)
                {
                    return;
                }
                annotation.IsDelegated = true;
                _context.Update(annotation);
                _context.SaveChanges();
            });


            //inter in case referal form Reciving OutGoing In case Of Accept Or Editing
            if (delegationDTO.acceptPreviousTRAR == true && delegationDTO.ActionId == (int)ActionEnum.ExternalDelegationExecute && delegationDTO.ReferrerTransactionActionId != null)
            {
                //update previous Recipients and set updated CC Tp False

                long View_Recipients_lst = _context.COUNTS.FromSqlInterpolated($"exec sp_Outgoing_GetExternalRecipientsOrganization {delegationDTO.ReferrerTransactionActionId.Value},{(int)RecipientStatusesEnum.Accepted},{_sessionServices.UserId},{null}").FirstOrDefault().CNT;
                bool result = false;
                if (View_Recipients_lst > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                if (result == false)
                {
                    return false;
                }
            }

            //Generate TransactionNumber if CreateTransactionNumberWhileRegistering is False
            Transaction currentTransacTion = _unitOfWork.GetRepository<Transaction>().GetById(delegationDTO.TransactionId);
            if ((bool)delegationDTO.IsTransaction)
            {
                string Format = _SystemSettingsService.GetSystemSettingByCode("CreateTransactionNumberWhileRegistering").SystemSettingValue.Trim().ToUpper();

                if (Format == "FALSE" && string.IsNullOrEmpty(currentTransacTion.TransactionNumber))
                {
                    bool update_status = UpdateTransactionNumber(ref currentTransacTion);
                    if (update_status == false)
                    {
                        return false;
                    }
                }
            }


            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction currentTransactionDB = _context.Database.BeginTransaction();
            using (currentTransactionDB)
            {
                try
                {
                    transactionActionLst.Add(transactionActionObj);

                    //_UnitOfWork.Repository<TransactionAction>().Insert(transactionAction);

                    _context.BulkInsert(transactionActionLst, new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true, UseTempDB = false });

                    int maxTransactionActionAttachmentId = (int)Count(HelperFunctionEnum.ActionAttachmentMaxIndexPlus, 0, 0, false, 0);
                    int i = maxTransactionActionAttachmentId;
                    //Fill transactionActionAttachmentList
                    foreach (TransactionAction TAction in transactionActionLst)
                    {
                        foreach (TransactionActionAttachment transactionActionAttcahment in TAction.TransactionActionAttachments)
                        {
                            if (_UnitOfWork.GetRepository<TransactionAttachment>().GetAll().Where(a => a.TransactionId == delegationDTO.TransactionId && a.TransactionAttachmentId == transactionActionAttcahment.TransactionAttachmentId).Count() <= 0)
                            {
                                if (System.IO.File.Exists("C:\\testDelegation.txt"))
                                {
                                    using (StreamWriter sw = System.IO.File.AppendText("C:\\testDelegation.txt"))
                                    {
                                        sw.WriteLine("Function is = TestDelegation");
                                        sw.WriteLine("TransactionId=" + delegationDTO.TransactionId);
                                        sw.WriteLine("TransactionAttachmentId=" + transactionActionAttcahment.TransactionAttachmentId);
                                        sw.WriteLine("sys Date=" + DateTime.Now.ToLongDateString() + "  -- sys Time=" + DateTime.Now.ToLongTimeString());
                                        sw.WriteLine("----------------- END -----------------------");
                                    }
                                }

                                return false;
                            }
                            transactionActionAttcahment.TransactionActionAttachmentId = i++;
                            transactionActionAttcahment.TransactionActionId = TAction.TransactionActionId;
                            transactionActionAttachmentList.Add(transactionActionAttcahment);
                        }
                    }
                    //_UnitOfWork.Repository<TransactionActionAttachment>().Insert(transactionActionAttachmentList);
                    _context.BulkInsert(transactionActionAttachmentList, new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true, UseTempDB = false });
                    transactionActionAttachmentList = new List<TransactionActionAttachment>();

                    int maxRecipientsIndex = (int)Count(HelperFunctionEnum.ActionRecipientMaxIndexPlus, 0, 0, false, 0);


                    //_unitOfWork.GetRepository<TransactionActionRecipient>().GetAll().Max(x => x.TransactionActionRecipientId);
                    i = maxRecipientsIndex;
                    //Fill transactionActionRecipientLst
                    foreach (TransactionAction TAction in transactionActionLst)
                    {
                        foreach (TransactionActionRecipient TARecipient in TAction.TransactionActionRecipients)
                        {
                            TARecipient.TransactionActionRecipientId = i++;
                            TARecipient.TransactionActionId = TAction.TransactionActionId; // setting FK to match its linked PK that was generated in DB
                                                                                           //  TARecipient.IsNoteHidden = delegationDTO.IsNoteHidden;                                                       //TARecipient.IsCC = TARecipient.IsCC; // setting FK to match its linked PK that was generated in DB

                            transactionActionRecipientLst.Add(TARecipient);
                        }

                        //  transactionActionRecipientLst = transactionActionRecipientLst.OrderByDescending(x => x.TransactionActionRecipientId).Select(x => x).ToList();
                        _context.BulkInsert(transactionActionRecipientLst, new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true, UseTempDB = false });
                        _context.SaveChanges();
                        if (!currentTransacTion.IsForAll)
                        {
                            int maxRecipientAttachmentsIndex = (int)Count(HelperFunctionEnum.ActionRecipientAttachmentMaxIndexPlus, 0, 0, false, 0);
                            //_unitOfWork.GetRepository<TransactionActionRecipientAttachment>().GetAll().Max(x => x.TransactionActionRecipientAttachmentId);
                            i = maxRecipientAttachmentsIndex;
                            //Fill subSubTransactionActionRecipientAttachList
                            foreach (TransactionActionRecipient TARecipient in transactionActionRecipientLst)
                            {
                                foreach (TransactionActionRecipientAttachment TARecipientAttachment in TARecipient.TransactionActionRecipientAttachments)
                                {
                                    TARecipientAttachment.TransactionActionRecipientAttachmentId = i++;
                                    TARecipientAttachment.TransactionActionRecipientId = TARecipient.TransactionActionRecipientId;
                                    subSubTransactionActionRecipientAttachList.Add(TARecipientAttachment);
                                }

                                // subSubTransactionActionRecipientAttachList = new List<TransactionActionRecipientAttachment>();
                            }
                            _context.BulkInsert(subSubTransactionActionRecipientAttachList, new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true, UseTempDB = true });
                            //transactionActionRecipientLst = new List<TransactionActionRecipient>();
                        }
                        else
                        {

                            foreach (TransactionAttachment _TAttachment in currentTransacTion.TransactionAttachments)
                            {
                                _TAttachment.IsShared = true;
                            }
                        }
                    }
                    // in case of delegated from Inbox
                    if (delegationDTO.ReferrerTransactionActionRecipientId != null
                        && delegationDTO.reject_fromPreparation != true
                        && delegationDTO.ApplyAcceptConfirmation != true
                        && delegationDTO.ActionId != (int)ActionEnum.Preparation
                        && delegationDTO.ActionId != (int)ActionEnum.Confirmation)
                    {
                        TransactionActionRecipient referral_recipient = _unitOfWork.GetRepository<TransactionActionRecipient>().GetAll()
                                                            .FirstOrDefault(w => w.TransactionActionRecipientId == delegationDTO.ReferrerTransactionActionRecipientId.Value);
                        if (referral_recipient != null && referral_recipient.TransactionAction.ActionId != (int)ActionEnum.Confirmation)
                        {
                            // referral_recipient.IsNoteHidden = delegationDTO.transactionActionRecipientsDTO.FirstOrDefault(w => w.TransactionActionRecipientId == delegationDTO.ReferrerTransactionActionRecipientId.Value).IsNoteHidden;
                            //   currentTransactionDB.Commit();
                            _UnitOfWork.SaveChanges();

                            //   _context.Dispose();

                            bool result = UpdateTransctionActionRecStatusByTARecipientId(new int[] { delegationDTO.ReferrerTransactionActionRecipientId.Value }, (currentTransacTion.TransactionTypeId.Equals(2) || currentTransacTion.TransactionTypeId.Equals(3) || referral_recipient.IsCc) ? (int)RecipientStatusesEnum.Seen : (int)RecipientStatusesEnum.Sent, null, null);
                            if (result == false)
                            {
                                currentTransactionDB.Rollback();
                                currentTransactionDB.Dispose();
                                return false;
                            }
                        }
                    }

                    // Update Max Receipient
                    //if (delegationDTO.IgnoreWorkFlowProcess)
                    //{
                    //    _context.COUNTS.FromSql($"exec sp_UpdateMaxTransactionActionReceipientId {delegationDTO.TransactionId},{delegationDTO.isEmployee},{DirectedFromOrganizationInCaseOfIgnoreWorkFlow },{_SessionServices.UserId}").ToList();
                    //}
                    //else
                    //{
                    _context.COUNTS.FromSqlInterpolated($"exec sp_UpdateMaxTransactionActionReceipientId {delegationDTO.TransactionId},{delegationDTO.isEmployee},{_sessionServices.OrganizationId },{_sessionServices.UserId}").ToList();
                    // }

                    //int count = _context.SaveChanges();


                    //bool countNOtification =  SendNotification(transactionActionRecipientLst, delegationDTO.TransactionId,  delegationDTO.ReferrerTransactionActionId,delegationDTO.ActionId, currentTransacTion.TransactionNumber,delegationDTO.ActionNumber,delegationDTO.acceptPreviousTRAR);


                    currentTransactionDB.Commit();

                    _UnitOfWork.SaveChanges();
                    // Mohamed Shaaban Dispose DbContext to Avoid Locking Context on Prepration
                    _context.Dispose();

                    // _context = new MasarContext();
                    //currentTransacTion = _context.Transactions.Find(delegationDTO.TransactionId);
                    // Mohamed Shaaban Reload DbContext to get Transaction's Related Entities
                    //_context.Entry(currentTransacTion).GetDatabaseValues();
                    //_context.Entry(currentTransacTion).Reload();

                    //  DelegateCustomization(delegationDTO.TransactionId, loggedInUser, loggedinOrganization, currentTransacTion.TransactionNumber);
                    // Send Notifications -- transactionActionRecipients is transactionActionRecipientLst
                    string IsForAll = _UnitOfWork.GetRepository<SystemSetting>().GetAll().FirstOrDefault(w => w.SystemSettingCode == "SendMailForAll")?.SystemSettingValue;
                    if (!currentTransacTion.IsForAll || IsForAll == "1")
                    {
                        if (currentTransacTion.TransactionType == null)
                            currentTransacTion.TransactionType = _UnitOfWork.GetRepository<TransactionType>().GetById(currentTransacTion.TransactionTypeId);
                        SendNotification(currentTransacTion, transactionActionRecipientLst, delegationDTO.TransactionId, delegationDTO.ReferrerTransactionActionId, delegationDTO.ActionId, currentTransacTion.TransactionNumberFormatted, delegationDTO.ActionNumber, delegationDTO.acceptPreviousTRAR, loggedInUser, loggedinOrganization, delegationDTO.IsTransaction);
                        // Send Email And SMS
                        //   SendEmailAndSMS(currentTransacTion, (int)transactionActionRecipientLst.First().TransactionActionId, transactionActionRecipientLst, loggedInUser, loggedinOrganization);
                    }
                    // Implemnt WorkFlow For Delegation
                    delegationDTO.TransactionActionId = (int)transactionActionRecipientLst.First().TransactionActionId;
                    var followups = _unitOfWork.GetRepository<FollowUp>().GetAll().Where(f => f.TransactionId == currentTransacTion.TransactionId).ToList();
                    List<Notification> notification_lst = new List<Notification>();
                    foreach (var followup in followups)
                    {
                        List<TransactionAction> trancationaction = _UnitOfWork.GetRepository<TransactionAction>()
                                           .GetAll()
                                           .Where(w => w.TransactionActionId == followup.TransactionActionId).ToList();
                        int maxNotificationIndex = (int)Count(HelperFunctionEnum.Notification, 0, 0, false, 0);
                        notification_lst = trancationaction.Select(x => new Notification
                        {
                            NotificationId = maxNotificationIndex++,
                            TransactionId = followup.TransactionId,
                            TransactionActionId = x.TransactionActionId,
                            //TransactionActionRecipientId = x.TransactionActionRecipientId,
                            NotificationTypeId = (int)NotificationTypeEnum.System,
                            UserId = followup.UserId,
                            OrganizationId = followup.OrganizationId,
                            CreatedBy = _sessionServices.UserId,
                            CreatedOn = DateTimeOffset.Now,
                            //ContentAr = (statusId == (int)RecipientStatusesEnum.Received) ? "تم استلام المعامله" : "تم حفظ المعاملة",
                            //ContentEn = (statusId == (int)RecipientStatusesEnum.Received) ? "Transaction has been recieved" : "Transaction has been Saved"
                            ContentAr = " تم احالة المعاملة رقم" + currentTransacTion.TransactionNumber,
                            ContentEn = "Transaction has been Delegated no " + currentTransacTion.TransactionNumber,

                        }).ToList();
                    }
                    _context.BulkInsert(notification_lst, new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true, UseTempDB = false });
                    //SendEmail(transaction, _TransactionactionRecipients, loggedInUser, loggedinOrganization);
                    // _context.SaveChanges();
                    //  implementWorkFlow(delegationDTO, currentTransacTion, transactionActionRecipientLst);


                    //Check If Is FollowUp Enabled
                    if (delegationDTO.isFollowUp != null && delegationDTO.isFollowUp.Value == true)
                    {
                        FollowUpDTO followUpDTO = new FollowUpDTO()
                        {
                            TransactionId = delegationDTO.TransactionId,
                            TransactionActionId = delegationDTO.TransactionActionId == 0 ? transactionActionRecipientLst.LastOrDefault().TransactionActionId : delegationDTO.TransactionActionId,
                            TransactionActionRecipientId = transactionActionRecipientLst.LastOrDefault().TransactionActionRecipientId,
                            OrganizationId = delegationDTO.followUpOrganizationId,
                            FinishedDate = delegationDTO.followUpfinishedDate.Value,

                        };
                        List<FollowUpDTO> followUpDTOList = new List<FollowUpDTO>();
                        followUpDTOList.Add(followUpDTO);
                        FollowUpService.AddFollowUp(followUpDTOList, true);
                    }
                    //Return TransactionActionRecipientsIds To Invoke them in Notifications
                    //transactionActionRecipientsIds.AddRange(transactionActionRecipientLst.Select(x => x.TransactionActionRecipientId.ToString()));
                    //_signalRServices.SendNotification(transactionActionRecipientsIds, "Notification Test");


                    //_signalRServices.RefreshAfterDelegation(_dataProtectService.Encrypt(delegationDTO.ReferrerTransactionActionRecipientId.ToString()), _SessionServices.OrganizationId, _SessionServices.UserId);
                    //#region audit record action
                    //if (IsAuditEnabled)
                    //{
                    //    AuditService auditService = new AuditService(_appSettings, _SessionServices, _contextAccessor);
                    //    try
                    //    {
                    //        auditService.SaveAuditTrailForDelgation(null, delegationDTO, "Delegation", "Added", currentTransacTion.TransactionNumberFormatted);
                    //    }
                    //    catch (Exception ex)
                    //    {


                    //    }


                    //}
                    //#endregion

                    // Check if user will save Committee minutes ---- التاكد من ان المستخدم يريد حفظ محضر اللجنة مع المعاملة
                    
                    
                    if (delegationDTO.IsSaveCommitteeMinutes)
                    {

                        SaveCommitteeMinuteToCurrentTransaction(commitId.Value, delegationDTO.TransactionId, transactionActionRecipientLst.FirstOrDefault().TransactionActionRecipientId);
                    }


                    return true;
                }
                catch (Exception ex)
                {
                    currentTransactionDB.Dispose();
                    currentTransactionDB.Rollback();
                    throw ex;
                }
            }

        }
        public bool UpdateTransctionActionRecStatusByTARecipientId(int[] _TARecipientId, int StatusId, string Notes, int? archiveReasonId, IDbContextTransaction CurrentlyTransaction = null)
        {
            //bool commit_here = CurrentlyTransaction == null ? true : false;
            //var currentTransaction = CurrentlyTransaction ?? this._Context.Database.BeginTransaction();

            try
            {

                //int RecipientStatusId = this._Context.RecipientStatuses.FirstOrDefault(x => x.RecipientStatusCode == operationName).RecipientStatusId;
                List<TransactionActionRecipientStatus> transactionActionRecipientStatusLst = new List<TransactionActionRecipientStatus>();
                for (int i = 0; i < _TARecipientId.Length; i++)
                {
                    TransactionActionRecipient transactionActionRecipient = _context.TransactionActionRecipients.FirstOrDefault(w => w.TransactionActionRecipientId == _TARecipientId[i]);

                    if (transactionActionRecipient == null)
                    {
                        continue;
                    }
                    // Check if TransactionActionRecipient Status if Not Sent  
                    if (transactionActionRecipient.RecipientStatusId == (int)RecipientStatusesEnum.Sent || transactionActionRecipient.RecipientStatusId == StatusId)
                    {
                        // في حالة تعقيب بعد الحفظ 
                        if (transactionActionRecipient.RecipientStatusId == (int)RecipientStatusesEnum.Saved)
                        {
                            _context.TransactionActionRecipientStatuses.LastOrDefault(t => t.TransactionActionRecipientId == transactionActionRecipient.TransactionActionRecipientId && t.ArchiveReasonId != null).Notes += $"\n{Notes}";
                            _context.SaveChanges();
                        }
                        continue;
                    }
                    else
                    {
                        TransactionAction transactionAction = _context.TransactionActions.FirstOrDefault(w => w.TransactionActionId == transactionActionRecipient.TransactionActionId);
                        /* Check If the Action Is Confirmation*/
                        if (transactionAction.ActionId == (int)ActionEnum.Confirmation)
                        {
                            if (StatusId == (int)RecipientStatusesEnum.Received)
                            {
                                StatusId = (int)RecipientStatusesEnum.AcceptConfirmation;
                            }
                            if (StatusId == (int)RecipientStatusesEnum.Rejected)
                            {
                                StatusId = (int)RecipientStatusesEnum.RejectConfirmation;
                            }
                            if (StatusId == (int)RecipientStatusesEnum.Viewed)
                            {
                                StatusId = (int)RecipientStatusesEnum.Received;
                            }
                        }
                        var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
                        var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();

                        if (transactionAction.ActionId == (int)ActionEnum.Preparation && StatusId == (int)RecipientStatusesEnum.Received)
                        {
                            // update referrer transactionActionReceipient
                            TransactionActionRecipient referrertransactionActionRecipient = _context.TransactionActionRecipients
                                .FirstOrDefault(w => w.TransactionActionRecipientId == transactionAction.ReferrerTransactionActionRecipientId);
                            referrertransactionActionRecipient.RecipientStatusChangedOn = DateTimeOffset.Now;
                            referrertransactionActionRecipient.RecipientStatusChangedBy = _sessionServices.UserId;
                            transactionActionRecipientStatusLst.Add(new TransactionActionRecipientStatus()
                            {
                                RecipientStatusId = StatusId,
                                TransactionActionRecipientId = referrertransactionActionRecipient.TransactionActionRecipientId,
                                Notes = Notes,
                                TransactionActionRecipient = referrertransactionActionRecipient,
                                CreatedBy = _sessionServices.UserId,
                                CreatedOn = DateTimeOffset.Now,
                                UserRoleId = userRole.UserRoleId
                            });
                        }

                        transactionActionRecipient.RecipientStatusId = StatusId;
                        transactionActionRecipient.RecipientStatusChangedOn = DateTimeOffset.Now;
                        transactionActionRecipient.UpdatedOn = DateTimeOffset.Now;
                        //transactionActionRecipient.CreatedOn = DateTimeOffset.Now;
                        transactionActionRecipient.RecipientStatusChangedBy = _sessionServices.UserId;
                        if (StatusId == (int)RecipientStatusesEnum.RelatedSent)
                            transactionActionRecipient.Notes = transactionActionRecipient.Notes + " " + Notes;
                        else if (!string.IsNullOrEmpty(Notes))
                            transactionActionRecipient.Notes = transactionActionRecipient.Notes;
                        else
                        {
                            if (archiveReasonId != null)
                            {
                                transactionActionRecipient.Notes = _context.ArchiveReasons.Find((int)archiveReasonId).ReasonNameAr;
                            }

                        }
                        TransactionActionRecipientStatus transactionActionRecipientStatus = new TransactionActionRecipientStatus()
                        {
                            RecipientStatusId = StatusId,
                            TransactionActionRecipientId = _TARecipientId[i],
                            Notes = Notes,
                            TransactionActionRecipient = transactionActionRecipient,
                            CreatedBy = _sessionServices.UserId,
                            CreatedOn = DateTimeOffset.Now,
                            ArchiveReasonId = archiveReasonId ?? null,
                            UserRoleId = userRole.UserRoleId
                        };
                        transactionActionRecipientStatusLst.Add(transactionActionRecipientStatus);
                    }
                }
                _context.TransactionActionRecipientStatuses.AddRange(transactionActionRecipientStatusLst);
                _context.SaveChanges();
                //if (commit_here == true)
                //{
                //    currentTransaction.Commit();
                //    currentTransaction.Dispose();
                //}
            }
            catch (System.Exception ex)
            {
                //currentTransaction.Rollback();
                //currentTransaction.Dispose();
                throw;
            }
            return true;
        }

        private bool UpdateTransactionNumber(ref Transaction currentTransacTion)
        {
            string addCodeValue = "";
            int TransactionTypeId = currentTransacTion.TransactionTypeId;
            bool AddOrgCodeToSerial = _UnitOfWork.GetRepository<TransactionType>().GetAll().Where(w => w.TransactionTypeId == TransactionTypeId).FirstOrDefault().AddOrgCodeToSerial;
            if (AddOrgCodeToSerial)
            {
                string addCharacterFormatForSerial = _UnitOfWork.GetRepository<SystemSetting>().GetAll().FirstOrDefault(w => w.SystemSettingCode == "addCharacterFormatForSerial")?.SystemSettingValue;
                string OrganizationCode = _UnitOfWork.GetRepository<Organization>().GetAll().Where(w => w.OrganizationId == _sessionServices.OrganizationId).FirstOrDefault().Code.ToString();
                addCodeValue = addCharacterFormatForSerial + OrganizationCode;
            }
            Random random = new Random();

            string TransactionNumberSerial = random.Next(0, int.MaxValue).ToString();
            //GetNextTransactionNumberSerial(currentTransacTion.TransactionTypeId).ToString();
            string transactionNumber = GenerateTransactionNumber(currentTransacTion.TransactionTypeId, TransactionNumberSerial);
            currentTransacTion.TransactionNumberFormatted = GenerateTransactionNumberFormat(currentTransacTion.TransactionTypeId, TransactionNumberSerial, addCodeValue);
            currentTransacTion.TransactionNumber = removeDelimiters(transactionNumber);
            try
            {
                //_context.Entry(currentTransacTion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _UnitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            //currentTransacTion.TransactionDate = currentTransacTion.TransactionDate == null ? DateTimeOffset.Now : currentTransacTion.TransactionDate;

        }
        //public void SendEmailAndSMS(Transaction transaction, int transactionActionId, List<TransactionActionRecipient> transactionactionRecipients, int? loggedInUser, int? loggedinOrganization, string currentUrl = null)
        //{
        //    bool isSecret = transaction.ConfidentialityLevel == null ? false : transaction.ConfidentialityLevel.IsConfidential;
        //    #region Email Params ,Mail Subject and Subject initialization

        //    EmailParamsDTO emailParams = new EmailParamsDTO
        //    {

        //        //رقم القيد
        //        lblTransactionNumber = LocalizationRepository.GetByKey("TransactionNumber", "ar"),
        //        //موضوع المعاملة
        //        lblTransactionSubject = LocalizationRepository.GetByKey("TransactionSubject", "ar"),
        //        //نوع المعاملة
        //        lblTransactionType = LocalizationRepository.GetByKey("TransactionType", "ar"),
        //        //تاريخ المعاملة 
        //        lblTransactionDate = LocalizationRepository.GetByKey("TransactionDate", "ar"),
        //        //الاداره المحيلة
        //        lblDelegationFromOrg = LocalizationRepository.GetByKey("IncomingOrganization", "ar"),
        //        // مستوي الاهمية
        //        lblImportance_level = LocalizationRepository.GetByKey("ImportanceLevel", "ar"),
        //        // مستوي السرية
        //        lblConfidentiality_level = LocalizationRepository.GetByKey("ConfidentialityLevel", "ar"),
        //        // رابط المعاملة
        //        lblTransURL = LocalizationRepository.GetByKey("transactionBoxUrl", "ar"),
        //        // رسالة الشكر
        //        ThanksLocalization = LocalizationRepository.GetByKey("thanksMail", "ar"),
        //        lblRequiredAction = LocalizationRepository.GetByKey("RequiredAction", "ar"),
        //        lblNotes = LocalizationRepository.GetByKey("Notes", "ar"),
        //        transactionActionRecipientToBeReplace = "$UNIQUEURL$",
        //        transactionActionRecipientToBeReplaceEn = "$UNIQUEURLEN$",

        //        RequiredActionToBeReplace = "{$UNIQUEREQUIREDACTION$}",
        //        RequiredActionToBeReplaceEn = "{$UNIQUEREQUIREDACTIONEN$}",
        //        NotesToBeReplace = "{$Notes}",
        //        NotesToBeReplaceEn = "{$NotesEn}",
        //        //رقم القيد بالانجليزية
        //        lblTransactionNumberEn = LocalizationRepository.GetByKey("TransactionNumber", "en"),
        //        //موضوع المعاملة بالانجليزية
        //        lblTransactionSubjectEn = LocalizationRepository.GetByKey("TransactionSubject", "en"),
        //        //نوع المعاملة بالانجليزية
        //        lblTransactionTypeEn = LocalizationRepository.GetByKey("TransactionType", "en"),
        //        //تاريخ المعاملة  بالانجليزية
        //        lblTransactionDateEn = LocalizationRepository.GetByKey("TransactionDate", "en"),
        //        //الاداره المحيلة بالانجليزية
        //        lblDelegationFromOrgEn = LocalizationRepository.GetByKey("IncomingOrganization", "en"),
        //        // مستوي الاهمية بالانجليزية
        //        lblImportance_levelEn = LocalizationRepository.GetByKey("ImportanceLevel", "en"),
        //        // مستوي السرية بالانجليزية
        //        lblConfidentiality_levelEn = LocalizationRepository.GetByKey("ConfidentialityLevel", "en"),
        //        // رابط المعاملة  بالانجليزية
        //        lblTransURLEn = LocalizationRepository.GetByKey("transactionBoxUrl", "en"),
        //        lblRequiredActionEn = LocalizationRepository.GetByKey("RequiredAction", "en"),
        //        lblNotesEn = LocalizationRepository.GetByKey("Notes", "en"),
        //        lblIncomingOutgoingOrganization = LocalizationRepository.GetByKey("IncomingOutgoingOrganization", "ar"),
        //        lblIncomingOutgoingOrganizationEn = LocalizationRepository.GetByKey("IncomingOutgoingOrganization", "en"),
        //        lblMailHeader = (_SessionServices.CultureIsArabic) ? LocalizationRepository.GetByKey("TransactionNotificationMailHeader", "ar") : LocalizationRepository.GetByKey("TransactionNotificationMailHeader", "en"),
        //        ThanksLocalization2 = (_SessionServices.CultureIsArabic) ? LocalizationRepository.GetByKey("ThanksLocalization2", "ar") : LocalizationRepository.GetByKey("ThanksLocalization2", "en"),
        //    };

        //    string Message = "";
        //    string mailSubject = "";
        //    string NameFrom = loggedInUser != null ? _SessionServices.EmployeeFullNameAr : _UnitOfWork.Repository<Organization>().GetAll().Where(w => w.OrganizationId == loggedinOrganization).FirstOrDefault().OrganizationNameAr;
        //    string NameFromEn = loggedInUser != null ? _SessionServices.EmployeeFullNameEn : _UnitOfWork.Repository<Organization>().GetAll().Where(w => w.OrganizationId == loggedinOrganization).FirstOrDefault().OrganizationNameEn;
        //    TransactionAction transactionAction = _UnitOfWork.Repository<TransactionAction>().GetById(transactionActionId, false);
        //    if (transactionAction.ActionId == (int)ActionEnum.ExternalDelegationRequest || transactionAction.ActionId == (int)ActionEnum.ExternalDelegationExecute)
        //    {
        //        List<vm_ExternalOrganizations_Names_ByTAId> orgListNames = organizationRepository.Get_ExternalOrganizations_Names_ByTAId(transactionActionId);
        //        emailParams.OrgNamesAr = String.Join(",", orgListNames.Select(x => x.OrganizationNameAr).ToArray());
        //        emailParams.OrgNamesEn = String.Join(",", orgListNames.Select(x => x.OrganizationNameEn).ToArray());
        //        emailParams.IsExternalDelegation = true;
        //        emailParams.lblExternalOrgsDelegate = LocalizationRepository.GetByKey("ExternalOrganizationsSentTo", "ar");
        //        emailParams.lblExternalOrgsDelegateEn = LocalizationRepository.GetByKey("ExternalOrganizationsSentTo", "en");
        //    }
        //    mailServices.getMailMessage(transaction, ref Message, ref mailSubject, NameFrom, NameFromEn, emailParams);

        //    #endregion



        //    #region SMS Params Body and Template
        //    // First Get Template Code
        //    SMSTemplate SMSTemplate_delegation = SMSTemplateRepository.getTemplateByCode(SMSTemplateCodes.delegation) ?? new SMSTemplate { };
        //    SMS_Text_Params_DTO SMSTemplateDTO = new SMS_Text_Params_DTO
        //    {
        //        TextMessage = SMSTemplate_delegation.TextMessage,
        //        ParamtersString = SMSTemplate_delegation.Parameters,
        //        TemplateCode = SMSTemplate_delegation.TemplateCode,
        //        IsActive = SMSTemplate_delegation.IsActive
        //    };
        //    SMSTemplate_delegation = null;

        //    // Second Create Fields
        //    SMSDelegationFieldsDTO Fields = new SMSDelegationFieldsDTO()
        //    {
        //        TransactionNumberFormatted = transaction.TransactionNumberFormatted,
        //        From = NameFrom,
        //        Date = DateTimeOffset.Now,

        //    };
        //    SMS_Text_Params_DTO message_body = SMSTemplateRepository.GetTemplateParams_Body(SMSTemplateDTO, Fields);
        //    #endregion

        //    // For Users Of Organizattions
        //    // Get All Organizations

        //    // Get User to Send SMS ...


        //    //var All_Organizations = transactionactionRecipients.Where(a => a.DirectedToOrganization != null).Select(a => a.DirectedToOrganizationId).ToList();




        //    /////transactionactionRecipients.Where(w => w.DirectedToOrganizationId != null)
        //    //                                                //.Select(s => new All_OrganizationsDTO
        //    //                                                //{
        //    //                                                //    organizationId = s.DirectedToOrganizationId.Value,
        //    //                                                //    TransactionActionRecipientId = s.TransactionActionRecipientId,
        //    //                                                //    Users = Get_Organization_Members_email_Mobile(s.DirectedToOrganizationId.Value.ToString(), isSecret)
        //    //                                                //}).ToList();

        //    List<View_Organization_MembersVm> _UsersInOrgs = Get_Organization_Members_email_Mobile(transactionActionId, isSecret).ToList();
        //    List<DbContexts.MasarContext.Models.Attachment> doc = new List<DbContexts.MasarContext.Models.Attachment>();
        //    doc = transaction.TransactionAttachments?.Select(x => x.Attachment).ToList();
        //    Task.Run(() =>
        //    {
        //        //Thread.CurrentThread.IsBackground = true;

        //        //foreach (TransactionActionRecipient Recipient in transactionactionRecipients)
        //        //{
        //        //    if (Recipient.DirectedToUserId != null)
        //        //    {
        //        //        if (Recipient.DirectedToUser.NotificationByMail)
        //        //        {
        //        //            if (!string.IsNullOrEmpty(Recipient.DirectedToUser.Email))
        //        //            {
        //        //    string requiredActionAr = _uow.Repository<RequiredAction>().GetById(Recipient.RequiredActionId).RequiredActionNameAr;
        //        //                string recipient_Url = url.Replace("transactionActionRecipientToBeReplaced", (Recipient.TransactionActionRecipientId*-1).ToString());
        //        //                string New_Message = Message.Replace("$UNIQUEURL$", recipient_Url).Replace("{$UNIQUEREQUIREDACTION$}", requiredActionAr);
        //        //                AlternateView htmlView = CreateAlternateView(New_Message, null, "text/html");
        //        //                mailServices.SendNotificationEmail(Recipient.DirectedToUser.Email, mailSubject, null, true, htmlView,"",Hosting.AngularRootPath);
        //        //            }


        //        //        }
        //        //        if (SMSTemplate_delegation.IsActive && message_body.TextMessage != "Error")
        //        //        {

        //        //            if (Recipient.DirectedToUser.NotificationBySMS && message_body.Paramters != null)
        //        //            {
        //        //                if (!string.IsNullOrEmpty(Recipient.DirectedToUser.Mobile))
        //        //                {
        //        //                    SmsServices.Send(Recipient.DirectedToUser.Mobile, message_body.Paramters.ToArray(), message_body.TextMessage, SMSTemplate_delegation.TemplateCode);

        //        //                }


        //        //            }
        //        //        }

        //        //    }

        //        //}



        //        //foreach (All_OrganizationsDTO organization in All_Organizations)
        //        //{
        //        if (string.IsNullOrEmpty(url))
        //        {
        //            url = currentUrl;
        //        }

        //        foreach (View_Organization_MembersVm user in _UsersInOrgs)
        //        {
        //            if (user.NotificationByMail)
        //            {
        //                if (!string.IsNullOrEmpty(user.Email))
        //                {
        //                    string recipient_Url = url.Replace("transactionActionRecipientToBeReplaced", _dataProtectService.Encrypt(user.TransactionActionRecipientId.ToString()));
        //                    string New_Message = Message.Replace("$UNIQUEURL$", recipient_Url)
        //                                                .Replace("$UNIQUEURLEN$", recipient_Url)
        //                                                .Replace("{$UNIQUEREQUIREDACTION$}", user.RequiredActionNameAr)
        //                                                .Replace("{$UNIQUEREQUIREDACTIONEN$}", user.RequiredActionNameEn);
        //                    if (user.Notes != null)
        //                    {
        //                        New_Message = New_Message.Replace("{$Notes}", user.Notes).Replace("{$NotesEn}", user.NotesEn)
        //                                                 .Replace("{displayOption}", "table-row");
        //                    }
        //                    else
        //                    {
        //                        New_Message = New_Message.Replace("{$Notes}", user.Notes).Replace("{$NotesEn}", user.Notes)
        //                                                 .Replace("{displayOption}", "none");
        //                    }
        //                    AlternateView htmlView = CreateAlternateView(New_Message, null, "text/html");
        //                    string fileName = "", mimeType = "";

        //                    var documnents = doc?.Select(x => new
        //                    {

        //                        docm = (_DocumentServices.GetDocumentContent(x.LFEntryId, ref fileName, ref mimeType, false)).Length == 0 ? _DocumentServices.GetDocumentContent(x.LFEntryId, ref fileName, ref mimeType, true) : _DocumentServices.GetDocumentContent(x.LFEntryId, ref fileName, ref mimeType, false),
        //                        Name = x.AttachmentName,
        //                        PageCount = x.PagesCount,
        //                    }).ToList();
        //                    string attachments = "";
        //                    var OfficeSetting = this._appSettings.Value.DocumentSettings.OfficeSetting;
        //                    string attachmentSetting = _SystemSettingsService.GetSystemSettingByCode("SendAttachmentForDelegation").SystemSettingValue;
        //                    if (documnents != null && (transaction.TransactionTypeId == 2 || transaction.TransactionTypeId == 3 || (transactionAction.ActionId == 4 && attachmentSetting == "1")))
        //                    {
        //                        foreach (var binary in documnents)
        //                        {
        //                            if (binary.docm != null && binary.docm.Length > 0)
        //                            {
        //                                //byte[] bytes;
        //                                var exe = binary.Name.Split(".")[1];
        //                                if ((exe.ToLower() == "doc".ToLower() || exe.ToLower() == "docx".ToLower() || exe.ToLower() == "xlsx".ToLower() || exe == "xls".ToLower()) && binary.PageCount == 0)
        //                                {
        //                                    var file_path = Path.Combine(Hosting.RootPath, "Documents\\" + Guid.NewGuid() + "." + exe);
        //                                    FileStream fs = new FileStream(file_path, FileMode.OpenOrCreate);
        //                                    fs.Write(binary.docm, 0, binary.docm.Length);
        //                                    fs.Close();
        //                                    attachments += file_path + ",";
        //                                }
        //                                else
        //                                {
        //                                    var file_path = Path.Combine(Hosting.RootPath, "Documents\\" + Guid.NewGuid() + "." + "tiff");
        //                                    FileStream fs = new FileStream(file_path, FileMode.OpenOrCreate);
        //                                    fs.Write(binary.docm, 0, binary.docm.Length);
        //                                    fs.Close();
        //                                    attachments += file_path + ",";
        //                                }
        //                            }
        //                        }

        //                    }

        //                    if (!string.IsNullOrEmpty(attachments))
        //                        attachments = attachments.Remove(attachments.Length - 1);
        //                    mailServices.SendNotificationEmail(user.Email, mailSubject, null, true, htmlView, "", Hosting.AngularRootPath, transaction, attachments);

        //                }
        //            }
        //            if (SMSTemplateDTO.IsActive && user.NotificationBySMS)
        //            {
        //                if (!string.IsNullOrEmpty(user.Mobile))
        //                {
        //                    SmsServices.Send(user.Mobile, message_body.Paramters.ToArray(), message_body.TextMessage, SMSTemplateDTO.TemplateCode);
        //                }
        //            }
        //        }

        //        ////}
        //        //string usserBedawyTafeel = "mbedawy@tafeel.com";
        //        //mailServices.SendNotificationEmail(usserBedawyTafeel, mailSubject, true, htmlView);
        //    });
        //}

        public long Count(HelperFunctionEnum caseNumber, int userId, int organizationId, bool isEmployee, int periodId)
        {
            return this._context.COUNTS.FromSqlRaw($"exec sp_GetCount {(int)caseNumber},{userId},{organizationId},{isEmployee},{periodId}").ToList().FirstOrDefault().CNT;
        }

        public List<Vw_ReturnGroupReferralDTO> GroupReferral(GroupReferralDTO groupReferralDTO)
        {
            List<Vw_ReturnGroupReferralDTO> result = new List<Vw_ReturnGroupReferralDTO>();
            try
            {
                if (!string.IsNullOrEmpty(groupReferralDTO.tranActionRecIds))
                {
                    List<string> recipient_arrBefore = groupReferralDTO.tranActionRecIds.Split(',').ToList();
                    string tranActionRecIdsAfterDecrypt = "";
                    recipient_arrBefore.ForEach(s =>
                    {
                        string transactionId = _dataProtectService.Decrypt(s.Split('_')[0]);
                        string transactionActionRecipientId = _dataProtectService.Decrypt(s.Split('_')[1]);
                        tranActionRecIdsAfterDecrypt = tranActionRecIdsAfterDecrypt + transactionId + "_" + transactionActionRecipientId + ",";
                    });
                    groupReferralDTO.tranActionRecIds = tranActionRecIdsAfterDecrypt.Remove(tranActionRecIdsAfterDecrypt.Length - 1, 1);
                }
                result = GroupReferralFunction(groupReferralDTO).ToList();
                return result;
            }
            catch (Exception Ex)
            {
                return new List<Vw_ReturnGroupReferralDTO>();
            }
        }
        private IQueryable<Vw_ReturnGroupReferralDTO> GroupReferralFunction(GroupReferralDTO groupReferralDTO)
        {
            try
            {
                groupReferralDTO.isEmp = false;

                string committeId  = Encription.DecryptStringAES(groupReferralDTO.FromCommiteeId);
                int ConvertedCommitteId  =int.Parse(committeId);

                //var res = _context.Vw_ReturnGroupReferralDTO.FromSqlInterpolated($"[dbo].[sp_GroupReferral] {groupReferralDTO.tranActionRecIds ?? (object)DBNull.Value},{groupReferralDTO.tranActionIds ?? (object)DBNull.Value},{groupReferralDTO.requiredActionId ?? (object)DBNull.Value},{groupReferralDTO.note},{groupReferralDTO.empCCIds ?? (object)DBNull.Value},{groupReferralDTO.orgCCIds ?? (object)DBNull.Value},{groupReferralDTO.directToEmpId ?? (object)DBNull.Value},{groupReferralDTO.directToOrgId ?? (object)DBNull.Value},{groupReferralDTO.actionId ?? (object)DBNull.Value},{_SessionServices.UserId ?? (object)DBNull.Value},{_SessionServices.UserRoleId},{(groupReferralDTO.isEmp ? _SessionServices.UserId : null)},{(groupReferralDTO.isEmp ? null : _SessionServices.OrganizationId ?? (object)DBNull.Value)},{groupReferralDTO.correspondentUserId ?? (object)DBNull.Value}");
                var res = _context.Vw_ReturnGroupReferralDTO.FromSqlInterpolated($"[dbo].[sp_GroupReferral] {groupReferralDTO.tranActionRecIds},{groupReferralDTO.tranActionIds},{groupReferralDTO.requiredActionId},{groupReferralDTO.note},{groupReferralDTO.empCCIds},{groupReferralDTO.orgCCIds},{groupReferralDTO.directToEmpId},{groupReferralDTO.directToOrgId},{groupReferralDTO.actionId},{_SessionServices.UserId},{_SessionServices.UserRoleIdOrignal},{(groupReferralDTO.isEmp ? _SessionServices.UserId : null)},{(groupReferralDTO.isEmp ? null : _SessionServices.OrganizationId)},{groupReferralDTO.correspondentUserId},{ConvertedCommitteId}");

                return res;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void SaveCommitteeMinuteToCurrentTransaction(int committeeId, long transactionId, int transactionActionRecipientId)
        {
            byte[] content = null;
            string committeeLocalize = _sessionServices.CultureIsArabic ?
                _unitOfWork.GetRepository<Localization>().GetAll().Where(l => l.Key == "committeeMinute").SingleOrDefault().ValueAr :
                _unitOfWork.GetRepository<Localization>().GetAll().Where(l => l.Key == "committeeMinute").SingleOrDefault().ValueEn;
            string pdfResolution = _SystemSettingsService.GetSystemSettingByCode("PDFResolution").SystemSettingValue;
            Boolean pdfToColor = Convert.ToBoolean(_SystemSettingsService.GetSystemSettingByCode("PDFToColor").SystemSettingValue);
            string ChromeExecutablePath = _SystemSettingsService.GetSystemSettingByCode("ChromeExecutablePath").SystemSettingValue;
            string MasarUrl = _SystemSettingsService.GetSystemSettingByCode("CommitteUrlForPdf").SystemSettingValue;

            var TokenValue = new StringValues();
            _contextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out TokenValue);

            GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
            ghostScriptFactory.ChromeExecutablePath = ChromeExecutablePath;
            ghostScriptFactory.masarUrl = MasarUrl;

            string CommitteeMinutesPdfPath =  ghostScriptFactory.UrlToPdfHeadless(TokenValue.FirstOrDefault().Replace("Bearer ", ""), committeeId, _sessionServices.UserName).Result;
            var pdfBomaryContent = File.ReadAllBytes(CommitteeMinutesPdfPath);

            string CommitteeMinutesTiffPath = ghostScriptFactory.ConvertPDFtoTIFF(pdfBomaryContent);
            content = System.IO.File.ReadAllBytes(CommitteeMinutesTiffPath);


            var attachmentEntity = new DocumentDTO()
            {
                AttachmentTypeId = 1,
                AttachmentName = $"{committeeLocalize}_{committeeId}",
                OriginalName = $"{committeeLocalize}_{committeeId}",
                MimeType = "image/tiff",
                Size = content.Length,
                BinaryContent = content
            };

            string[] param = new string[]
            {
                "1",
                transactionId.ToString()
            };

            var insertedAttachment = _IDocumentServices.InsertAttachment(attachmentEntity, param);

            var transactionAttachment = new TransactionAttachment
            {
                TransactionId = transactionId,
                AttachmentId = insertedAttachment.AttachmentId
            };

            _unitOfWork.GetRepository<TransactionAttachment>().Insert(transactionAttachment);
            _unitOfWork.GetRepository<TransactionActionRecipientAttachment>().Insert(new TransactionActionRecipientAttachment
            {
                TransactionActionRecipientId = transactionActionRecipientId,
                TransactionAttachmentId = transactionAttachment.TransactionAttachmentId
            });

            ghostScriptFactory.DeleteCopiedFiles(_sessionServices.UserName);
        }
    }
}
