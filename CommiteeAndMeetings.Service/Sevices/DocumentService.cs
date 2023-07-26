using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Hosting;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeAndMeetings.UI.Helpers;
using CommiteeDatabase.Models.Domains;
using HelperServices;
using HelperServices.Hubs;
using IHelperServices;
using IHelperServices.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Models;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class DocumentService : BusinessService<SavedAttachment, DocumentDTO>, IDocumentService
    {
        private IDocumentManagerServices _DocumentServices;
        private IFileServices _FileServices;
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Commitee> committeRepository;
        private readonly ICommitteAttachmentService _transactionAttachService;
        private readonly IRepository<SavedAttachment> attachmentRepository;
        private readonly IRepository<SurveyAttachment> _SurveyAttachmentRepository;
        private readonly IRepository<CommitteeTaskAttachment> _CommitteeTaskAttachment;
        //private readonly IRepository<CommentAttachmentInTask> _CommentAttachmentInTask;
        private SignalRHelper _signalR;
        private readonly IRepository<Annotation> annotationRepository;
        private readonly ISystemSettingsService _SystemSettingsService;
        private readonly IRepository<UserRole> _userRoles;
        private readonly IRepository<ECMArchiving> _eCMArchiving;
        private readonly IRepository<CommiteeSavedAttachment> _CommitteAttachmentRepository;

        IHelperServices.ISessionServices _sessionServices;
        ICommitteeNotificationService _committeeNotificationService;
        ICommiteeLocalizationService _commiteeLocalizationService;

        public DocumentService(IUnitOfWork unitOfWork, IMapper mapper, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IStringLocalizer stringLocalizer, IDocumentManagerServices documentServices, ISystemSettingsService systemSettingsService, IFileServices fileServices, IOptions<AppSettings> appSettings, ICommitteeNotificationService committeeNotificationService, ICommiteeLocalizationService commiteeLocalizationService, IHubContext<SignalRHub> signalR, IDataProtectService dataProtectService)
            : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _uow = base._UnitOfWork;
            _DocumentServices = documentServices;
            _SystemSettingsService = systemSettingsService;
            _FileServices = fileServices;
            committeRepository = _uow.GetRepository<Commitee>();
            _SurveyAttachmentRepository = _uow.GetRepository<SurveyAttachment>();
            _CommitteeTaskAttachment = _uow.GetRepository<CommitteeTaskAttachment>();
           // _CommentAttachmentInTask = _uow.GetRepository<CommentAttachmentInTask>();
            attachmentRepository = _uow.GetRepository<SavedAttachment>();
            annotationRepository = _uow.GetRepository<Annotation>();
            _userRoles = _uow.GetRepository<UserRole>();
            _eCMArchiving = _uow.GetRepository<ECMArchiving>();
            _CommitteAttachmentRepository = _uow.GetRepository<CommiteeSavedAttachment>();
            _sessionServices = sessionServices;
            _committeeNotificationService = committeeNotificationService;
            _commiteeLocalizationService = commiteeLocalizationService;
            _signalR = new SignalRHelper(signalR, dataProtectService);
        }

        /// <summary>
        /// Returns the same overridden function's result after assigning the
        /// BinaryContent property with the file's content from the file system
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileName"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public byte[] Download(object id, ref string fileName, ref string mimeType, bool getOriginal = false)
        {
            if (id == null)
            {
                return null;
            }
            //
            DocumentDTO entity = base.GetDetails(id);
            byte[] binaryContent = entity != null && !string.IsNullOrEmpty(entity.LFEntryId) ? _DocumentServices.GetDocumentContent(entity.LFEntryId, ref fileName, ref mimeType, getOriginal) : null;

            if (entity.MimeType.Equals("application/msword") ||
                entity.MimeType.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document") ||
                entity.MimeType.Equals("application/vnd.ms-excel") ||
                entity.MimeType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") ||
                entity.MimeType.Equals("application/octet-stream"))
            {
                mimeType = entity.MimeType;
            }

            if (binaryContent != null)
            {
                if (entity != null && !string.IsNullOrEmpty(entity.OriginalName))
                {
                    if (getOriginal)
                    {
                        fileName = (entity.OriginalName.ToLower().EndsWith(".tiff") || entity.OriginalName.ToLower().EndsWith(".tif")
                        || entity.OriginalName.ToLower().EndsWith(".wav") || entity.OriginalName.ToLower().EndsWith(".m4a")
                        || entity.OriginalName.ToLower().EndsWith(".doc") || entity.OriginalName.ToLower().EndsWith(".docx")
                        || entity.OriginalName.ToLower().EndsWith(".xls") || entity.OriginalName.ToLower().EndsWith(".xlsx")) ? entity.OriginalName : $"{entity.OriginalName}.tiff";
                    }
                    else
                    {
                        fileName = $"{Path.GetFileNameWithoutExtension(fileName)}.tiff";
                        //fileName = entity.OriginalName ?? "not assigned";
                    }
                }
                else
                {
                    fileName = "not assigned";
                }
                //if (string.IsNullOrEmpty(mimeType))
                //{
                //    mimeType = entity.MimeType ?? _FileServices.GetFileMimeType(binaryContent);
                //}                
            }
            return binaryContent;
        }

        /// <summary>
        /// Returns the same overridden function's result after assigning the
        /// BinaryContent property with the file's content from the file system
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileName"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        /// 
        public byte[] DownloadTransaction(object id, ref string fileName, ref string mimeType, bool getOriginal = false)
        {
            if (id == null)
            {
                return null;
            }
            //
            Attachment entity = _uow.GetRepository<Attachment>().GetById(id);
            byte[] binaryContent = entity != null && !string.IsNullOrEmpty(entity.LfentryId) ? _DocumentServices.GetDocumentContent(entity.LfentryId, ref fileName, ref mimeType, getOriginal) : null;

            if (entity.MimeType.Equals("application/msword") ||
                entity.MimeType.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document") ||
                entity.MimeType.Equals("application/vnd.ms-excel") ||
                entity.MimeType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") ||
                entity.MimeType.Equals("application/octet-stream"))
            {
                mimeType = entity.MimeType;
            }

            if (binaryContent != null)
            {
                if (entity != null && !string.IsNullOrEmpty(entity.OriginalName))
                {
                    if (getOriginal)
                    {
                        fileName = (entity.OriginalName.ToLower().EndsWith(".tiff") || entity.OriginalName.ToLower().EndsWith(".tif")
                        || entity.OriginalName.ToLower().EndsWith(".wav") || entity.OriginalName.ToLower().EndsWith(".m4a")
                        || entity.OriginalName.ToLower().EndsWith(".doc") || entity.OriginalName.ToLower().EndsWith(".docx")
                        || entity.OriginalName.ToLower().EndsWith(".xls") || entity.OriginalName.ToLower().EndsWith(".xlsx")) ? entity.OriginalName : $"{entity.OriginalName}.tiff";
                    }
                    else
                    {
                        fileName = $"{Path.GetFileNameWithoutExtension(fileName)}.tiff";
                        //fileName = entity.OriginalName ?? "not assigned";
                    }
                }
                else
                {
                    fileName = "not assigned";
                }
                //if (string.IsNullOrEmpty(mimeType))
                //{
                //    mimeType = entity.MimeType ?? _FileServices.GetFileMimeType(binaryContent);
                //}                
            }
            return binaryContent;
        }
        public byte[] DownloadPdf(object id, ref string fileName, ref string mimeType)
        {
            if (id == null)
            {
                return null;
            }
            //
            DocumentDTO entity = base.GetDetails(id);
            byte[] binaryContent = entity != null && !string.IsNullOrEmpty(entity.LFEntryId) ? _DocumentServices.GetDocumentContentPdf(entity.LFEntryId, ref fileName, ref mimeType) : null;
            //
            if (binaryContent != null)
            {
                if (entity != null && !string.IsNullOrEmpty(entity.OriginalName))
                {
                    fileName = (entity.OriginalName.ToLower().EndsWith(".pdf")) ? entity.OriginalName : $"{entity.OriginalName}.pdf";
                    //fileName = entity.OriginalName ?? "not assigned";
                }
                else
                {
                    fileName = "not assigned";
                }

                //mimeType = "pdf";
                //if (string.IsNullOrEmpty(mimeType))
                //{
                //    mimeType = entity.MimeType ?? _FileServices.GetFileMimeType(binaryContent);
                //}
                //
                //if (string.IsNullOrEmpty(mimeType))
                //{
                //mimeType = _FileServices.GetFileMimeType(binaryContent);
                //}
            }
            return binaryContent;
        }

        public byte[] DownloadPage(object id, int pageNumber, bool thumb)
        {
            if (id == null)
            {
                return null;
            }
            //
            DocumentDTO entity = base.GetDetails(id);
            byte[] binaryContent = entity != null && !string.IsNullOrEmpty(entity.LFEntryId) ? _DocumentServices.GetPageContent(entity.LFEntryId, pageNumber, thumb) : null;
            return binaryContent;
        }

        public byte[] DownloadPageOriginal(object id, int pageNumber, bool thumb)
        {
            if (id == null)
            {
                return null;
            }
            //
            DocumentDTO entity = base.GetDetails(id);
            byte[] binaryContent = entity != null && !string.IsNullOrEmpty(entity.LFEntryId) ? _DocumentServices.GetPageContentOriginal(entity.LFEntryId, pageNumber, thumb) : null;
            return binaryContent;
        }
        public byte[] DownloadPageOriginalForTransaction(object id, int pageNumber, bool thumb)
        {
            if (id == null)
            {
                return null;
            }
            //
            Attachment entity = _uow.GetRepository<Attachment>().GetById(id);
            byte[] binaryContent = entity != null && !string.IsNullOrEmpty(entity.LfentryId) ? _DocumentServices.GetPageContentOriginal(entity.LfentryId, pageNumber, thumb) : null;
            return binaryContent;
        }


        public bool RotatePage(int attachmentId, int pageNumber, int rotation)
        {
            if (string.IsNullOrEmpty(Convert.ToString(attachmentId)))
            {
                return false;
            }
            DocumentDTO entity = base.GetDetails(attachmentId);
            return _DocumentServices.RotatePage(Convert.ToInt32(entity.LFEntryId), pageNumber, rotation);
        }


        public CommiteeAttachmentDTO insertCommitteeAttachment(CommiteeSavedAttachment commiteeAttachment)
        {
            var CommiteeAttachment = _CommitteAttachmentRepository.Insert(commiteeAttachment);
            CommiteeAttachment.CreatedByRole = _uow.GetRepository<CommiteeUsersRole>().GetAll().Where(x => x.UserId == commiteeAttachment.CreatedBy && x.CommiteeId == commiteeAttachment.CommiteeId).FirstOrDefault();
            if (commiteeAttachment.AllUsers)
            {
                var committemebers = _UnitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.CommiteeId == commiteeAttachment.CommiteeId).ToList();
                foreach (var item in committemebers)
                {
                    var loc = _UnitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddNewAttachmentNotificationText");
                    if (item.UserId != _sessionServices.UserId)
                    {
                        CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                        {
                            IsRead = false,
                            UserId = (int)item.UserId,
                            TextAR = loc.CommiteeLocalizationAr + " " + commiteeAttachment.Description,
                            TextEn = loc.CommiteeLocalizationEn + " " + commiteeAttachment.Description,
                            CommiteeSavedAttachmentId = commiteeAttachment.CommiteeAttachmentId,
                            CommiteeId = commiteeAttachment.CommiteeId
                        };
                        List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                        _committeeNotificationService.Insert(committeeNotifications);
                    }

                }
            }
            else
            {
                foreach (var item in commiteeAttachment.AttachmentUsers)
                {
                    var loc = _UnitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddNewAttachmentNotificationText");
                    if (item.UserId != _sessionServices.UserId)
                    {
                        CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                        {
                            IsRead = false,
                            UserId = (int)item.UserId,
                            TextAR = loc.CommiteeLocalizationAr + " " + item.CommiteeAttachment.Description,
                            TextEn = loc.CommiteeLocalizationEn + " " + item.CommiteeAttachment.Description,
                            CommiteeSavedAttachmentId = item.CommiteeAttachmentId,
                            CommiteeId = commiteeAttachment.CommiteeId
                        };
                        List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                        _committeeNotificationService.Insert(committeeNotifications);
                    }
                }
            }

            return _Mapper.Map(CommiteeAttachment, typeof(CommiteeSavedAttachment), typeof(CommiteeAttachmentDTO)) as CommiteeAttachmentDTO;
        }

        //For InsertCommentAttachment

        public SavedAttachmentDTO InsertCommentAttachment(SavedAttachment Attachment)
        {
            var SavedAttachment = attachmentRepository.Insert(Attachment);
            SavedAttachment.CreatedByRole = _uow.GetRepository<CommiteeUsersRole>().Find(SavedAttachment.CreatedByRoleId);
           // SavedAttachment.Comment=_uow.GetRepository<Comment>().Find(SavedAttachment.Comment);
           

            return _Mapper.Map(SavedAttachment, typeof(SavedAttachment), typeof(SavedAttachmentDTO)) as SavedAttachmentDTO;
        }


        public SavedAttachmentDTO Insert(DocumentDTO entity, params string[] include)
        {
            int TransactionTypeId = 0;
            int TransactionId = 0;
            if (include.Count() == 2)
            {
                int.TryParse(include[0], out TransactionTypeId);
                int.TryParse(include[1], out TransactionId);
            }
            IEnumerable<DocumentDTO> result;
            if (entity.BinaryContent != null && entity.BinaryContent.Length > 0)
            {
                Commitee _transaction = committeRepository.GetById(TransactionId, true);
                var FileName = entity.AttachmentName ?? entity.OriginalName;
                entity.LFEntryId = _DocumentServices.CreateDocument(1, TransactionId, TransactionTypeId, FileName, entity.MimeType, entity.BinaryContent, out int PagesCount, _transaction);
                entity.PagesCount = PagesCount;
                result = base.Insert(new List<DocumentDTO>() { entity });
                entity.BinaryContent = null;//to not returning the BinaryContent with the entity after it's been saved.
            }
            else
            {
                result = base.Insert(new List<DocumentDTO>() { entity });
            }
            var doc = result.First();
            var attachmentSummaryDTO = new SavedAttachmentDTO
            {
                SavedAttachmentId = doc.SavedAttachmentId,
                AttachmentName = doc.AttachmentName,
                AttachmentTypeId = doc.AttachmentTypeId,
                LFEntryId = doc.LFEntryId,
                MimeType = doc.MimeType,
                Notes = doc.Notes,
                PagesCount = doc.PagesCount,
                Height = doc.Height,
                OriginalName = doc.OriginalName,
                PhysicalAttachmentTypeId = doc.ReferenceAttachmentId,
                Size = doc.Size,
                Width = doc.Width,
                CommentId=doc.CommentId
            };



            //var shareAttachmentsSetting = this._SystemSettingsService.GetSystemSettingByCode("ShareAttachmentsByDefault");
            //if (shareAttachmentsSetting != null)
            //{
            //    attachmentSummaryDTO.IsShared = shareAttachmentsSetting.SystemSettingValue == "1" ? true : false;
            //}
            return attachmentSummaryDTO;
        }

        public void InsertElecDoc(int entryId, byte[] officeBinaryContent, string extension)
        {
            if (officeBinaryContent != null && officeBinaryContent.Length > 0)
            {
                _DocumentServices.CreateElecDoc(entryId, officeBinaryContent, extension);
            }
        }


        public bool DeletePage(string id, int pageIndex)
        {
            SavedAttachment _attachment = attachmentRepository.GetById(id);
            if (_attachment == null)
            {
                return false;
            }
            bool IsPageDeleted = _DocumentServices.DeletePage(_attachment.LFEntryId, pageIndex);
            if (IsPageDeleted)
            {
                _attachment.PagesCount = _attachment.PagesCount - 1;
                attachmentRepository.Update(_attachment);
            }
            return IsPageDeleted;

        }
        public CommiteeAttachmentDTO MovePageTo(string CommiteeId, string sourceAttachID, int pageIndex, int moveLocation)
        {
            SavedAttachment _attachment = attachmentRepository.GetById(sourceAttachID);
            //Vw_Attachments transactionAttachmentOriginal = _CommitteAttachmentRepository.GetTransactionAttachments(Convert.ToInt64(transactionID)).Where(x => x.AttachmentId == Convert.ToInt32(sourceAttachID)).FirstOrDefault();
            //if (_attachment == null || transactionAttachmentOriginal == null)
            //{
            //    return new CommiteeAttachmentDTO();
            //}
            bool IsPageMoved = _DocumentServices.MovePageTo(_attachment.LFEntryId, pageIndex, moveLocation);
            if (IsPageMoved)
            {
                CommiteeAttachmentDTO savedTransAttachment = new CommiteeAttachmentDTO
                {
                    CommiteeId = Convert.ToInt32(CommiteeId),
                    //Attachments = attachmentList.Select(x => new SavedAttachment
                    //{
                    //    AttachmentName = x.AttachmentName,
                    //    AttachmentTypeId = x.AttachmentTypeId,
                    //    BinaryContent = x.BinaryContent,
                    //    Height = x.Height,
                    //    IsDisabled = x.IsDisabled,
                    //    LFEntryId = x.LFEntryId,
                    //    MimeType = x.MimeType,
                    //    Notes = x.Notes,
                    //    OriginalName = x.OriginalName,
                    //    PagesCount = x.PagesCount,
                    //    PhysicalAttachmentTypeId = x.PhysicalAttachmentTypeId,
                    //    SavedAttachmentId = x.SavedAttachmentId,
                    //    Size = x.Size,
                    //    Width = x.Width
                    //}).ToList(),
                    AllUsers = true,
                    CommiteeAttachmentId = _attachment.SavedAttachmentId,
                };
                IQueryable<Annotation> attachAnnotation = annotationRepository.GetAll().Where(x => x.AttachmentId == Convert.ToInt32(sourceAttachID));
                foreach (var item in attachAnnotation)
                {
                    if (item.PageNumber == pageIndex)
                    {
                        item.PageNumber = moveLocation;
                        annotationRepository.Update(item);
                        continue;
                    }
                    if (moveLocation > pageIndex)
                    {
                        for (int i = pageIndex + 1; i <= moveLocation; i++)
                        {
                            if (item.PageNumber == i)
                                item.PageNumber = item.PageNumber - 1;
                        }
                    }
                    else if (moveLocation < pageIndex)
                    {
                        for (int i = pageIndex - 1; i >= moveLocation; i--)
                        {
                            if (item.PageNumber == i)
                                item.PageNumber = item.PageNumber + 1;
                        }
                    }
                    annotationRepository.Update(item);
                }
                return savedTransAttachment;
            }
            return new CommiteeAttachmentDTO();
        }


        public IEnumerable<CommiteeAttachmentDTO> SplitDocument(string CommiteeId, string sourceAttachID, int fromPageIndex, int toPageIndex)
        {
            SavedAttachment _attachment = attachmentRepository.GetById(sourceAttachID);
            string AttachmentName = "";
            //Vw_Attachments transactionAttachmentOriginal = _transactionAttachmentRepository.GetTransactionAttachments(Convert.ToInt64(transactionID)).Where(x => x.AttachmentId == Convert.ToInt32(sourceAttachID)).FirstOrDefault();
            string AddIndexToAttachment = _SystemSettingsService.GetSystemSettingByCode("AddIndexToAttachment").SystemSettingValue;
            //if (_attachment == null || transactionAttachmentOriginal == null)
            //{
            //    return new List<TransactionAttachmentDTO>();
            //}
            string targetID = _DocumentServices.SplitDocument(_attachment.LFEntryId, fromPageIndex, toPageIndex);
            if (!string.IsNullOrEmpty(targetID))
            {
                List<TransactionAttachmentDTO> transAttachEntities = new List<TransactionAttachmentDTO>();
                transAttachEntities.Add(new TransactionAttachmentDTO
                {
                    TransactionId =long.Parse(CommiteeId),
                    EntryId = Convert.ToInt32(targetID),
                    //IsShared = false,
                });
                int i = 1;
                string AttachmentIndex = "";


                if (_attachment.AttachmentName.Contains("-") && AddIndexToAttachment == "1")
                {
                    int index = GetCountOfAttachment(Convert.ToInt32(CommiteeId)) + i;
                    AttachmentIndex = index.ToString() + " - ";
                    AttachmentName = (_attachment.AttachmentName).Split("-")[1];
                }
                List<CommiteeAttachmentDTO> savedTransAttachment = new List<CommiteeAttachmentDTO>();
                savedTransAttachment.Add(new CommiteeAttachmentDTO
                {
                    CommiteeId = Convert.ToInt32(CommiteeId),
                    //Attachments = attachmentList.Select(x => new SavedAttachment
                    //{
                    //    AttachmentName = x.AttachmentName,
                    //    AttachmentTypeId = x.AttachmentTypeId,
                    //    BinaryContent = x.BinaryContent,
                    //    Height = x.Height,
                    //    IsDisabled = x.IsDisabled,
                    //    LFEntryId = x.LFEntryId,
                    //    MimeType = x.MimeType,
                    //    Notes = x.Notes,
                    //    OriginalName = x.OriginalName,
                    //    PagesCount = x.PagesCount,
                    //    PhysicalAttachmentTypeId = x.PhysicalAttachmentTypeId,
                    //    SavedAttachmentId = x.SavedAttachmentId,
                    //    Size = x.Size,
                    //    Width = x.Width
                    //}).ToList(),
                    AllUsers = true,

                    CommiteeAttachmentId = _attachment.SavedAttachmentId,
                });
                //  IEnumerable<CommiteeAttachmentDTO> transactionAttachmentSplit = _transactionAttachService.InsertAttachmentCopyWithEntryID(transAttachEntities);
                //   savedTransAttachment.AddRange(transactionAttachmentSplit);

                IEnumerable<Annotation> sourceAttachAnnotation = annotationRepository.GetAll().Where(x => x.AttachmentId == Convert.ToInt32(sourceAttachID)).ToList();

                IEnumerable<Annotation> targetAttachAnnotation = annotationRepository.GetAll().Where(x => x.AttachmentId == Convert.ToInt32(sourceAttachID)
                && x.PageNumber >= fromPageIndex && x.PageNumber <= toPageIndex).ToList();

                foreach (var item in sourceAttachAnnotation)
                {
                    if (item.PageNumber > toPageIndex)
                    {
                        item.PageNumber = item.PageNumber - ((toPageIndex - fromPageIndex) + 1);
                        annotationRepository.Update(item);
                    }
                }
                foreach (var item in targetAttachAnnotation)
                {
                    //    item.AttachmentId = transactionAttachmentSplit.ElementAt(0).AttachmentId;
                    item.PageNumber = item.PageNumber - (fromPageIndex - 1);
                    annotationRepository.Update(item);
                }
                _attachment.PagesCount = _attachment.PagesCount - ((toPageIndex - fromPageIndex) + 1);
                attachmentRepository.Update(_attachment);
                //  savedTransAttachment.Where(x => x.AttachmentId == _attachment.SavedAttachmentId).SingleOrDefault().PagesCount = _attachment.PagesCount;

                return savedTransAttachment;
            }
            return new List<CommiteeAttachmentDTO>();
        }


        public List<string> GetTemplatesNames()
        {
            return _DocumentServices.GetTemplatesNames();
        }

        public int getTemplateIDByName(string temName)
        {
            return _DocumentServices.getTemplateIDByName(temName);
        }

        public IEnumerable<dynamic> GetEntries(int? entryId)
        {
            return _DocumentServices.GetEntries(entryId);
        }

        public IEnumerable<dynamic> GetEntriesForTransAttachments(int userRoleId, string fentryid, string searchText, bool isEmployee = false)
        {
            //get userroleid ECMArchiving folders
            UserRole _userRole = null;
            if (isEmployee)
            {
                int? empRolId = _UnitOfWork.GetRepository<Role>().GetAll(false).FirstOrDefault(r => r.IsEmployeeRole == true)?.RoleId;
                _userRole = _UnitOfWork.GetRepository<UserRole>().GetAll()
                                               .FirstOrDefault(r => /*r.UserId == _SessionServices.UserId.Value &&*/ r.RoleId == empRolId);
            }
            else
            {
                _userRole = _userRoles.GetAll().Where(x => x.UserRoleId == userRoleId).FirstOrDefault();
            }
            string folderEntryIds = "";
            if (_userRole != null && string.IsNullOrEmpty(fentryid))
            {
                if (_userRole.Role.IsEmployeeRole)
                    folderEntryIds = String.Join(",", _eCMArchiving.GetAll().Where(x => x.UserId == _userRole.UserId).Select(item => item.FolderEntryId));
                else
                    folderEntryIds = String.Join(",", _eCMArchiving.GetAll().Where(x => x.OrganizationId == _userRole.OrganizationId).Select(item => item.FolderEntryId));
            }
            else
            {
                folderEntryIds = fentryid;
            }
            return _DocumentServices.GetEntriesForTransAttachments(folderEntryIds, searchText);
        }


        public int GetCountOfAttachment(int CommiteeId)
        {
            return _CommitteAttachmentRepository.GetAll().Where(t => t.CommiteeId == CommiteeId && t.DeletedBy == null && t.DeletedOn == null).Count();
        }

        
        public int CountOfAttachmentComment(int CommentId)
        {
            return attachmentRepository.GetAll().Where(t => t.CommentId == CommentId).Count();
        }

        public byte[] GetDocumentContent(int? entryId, ref string fileName, ref string mimeType, bool getOriginal = false)
        {
            if (entryId == null)
            {
                return null;
            }
            //
            DocumentDTO entity = base.GetDetails(entryId);
            byte[] binaryContent = entity != null && !string.IsNullOrEmpty(entity.LFEntryId) ? _DocumentServices.GetDocumentContent(entity.LFEntryId, ref fileName, ref mimeType, getOriginal) : null;
            return binaryContent;
        }
        public Commitee GetCommiteeById(long transactionId)
        {
            return _uow.GetRepository<Commitee>().GetById(transactionId);
        }

        public byte[] CreateMsgFile(string FromMail, string FromName, string ToMail, string Subject, string Body, string Attatchments, string fileName)
        {
            Spire.Email.MailAddress addressFrom = new Spire.Email.MailAddress(FromMail, FromName);

            Spire.Email.MailAddress addressTo = new Spire.Email.MailAddress(ToMail);

            Spire.Email.MailMessage mail = new Spire.Email.MailMessage(addressFrom, addressTo);
            mail.Subject = Subject;

            string htmlString = Body;
            mail.BodyHtml = htmlString;
            if (Attatchments != "" && Attatchments != null || Attatchments != string.Empty)
            {
                foreach (var item in Attatchments.Split(","))
                {
                    mail.Attachments.Add(new Spire.Email.Attachment(item));
                }

            }
            //mail.Attachments.Add(new Spire.Email.Attachment("logo.png"));
            var filepath = Path.Combine(Hosting.RootPath, "Documents\\" + Guid.NewGuid() + ".msg");
            mail.Save(filepath, Spire.Email.MailMessageFormat.Msg);
            byte[] buff = null;
            FileStream fs = new FileStream(filepath,
                                           FileMode.Open,
                                           FileAccess.ReadWrite);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(filepath).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }

        public SurveyAttachmentDTO InsertSurveyAttachment(SurveyAttachment surveyAttachmentDTO)
        {
            var SurveyAttachment = _SurveyAttachmentRepository.Insert(surveyAttachmentDTO);

            return _Mapper.Map(surveyAttachmentDTO, typeof(SurveyAttachment), typeof(SurveyAttachmentDTO)) as SurveyAttachmentDTO;
        }

        public CommitteeTaskAttachmentDTO InsertCommitteeTaskAttachment(CommitteeTaskAttachment committeeTaskAttachment)
        {
            var TaskAttachment = _CommitteeTaskAttachment.Insert(committeeTaskAttachment);

            return _Mapper.Map(committeeTaskAttachment, typeof(CommitteeTaskAttachment), typeof(CommitteeTaskAttachmentDTO)) as CommitteeTaskAttachmentDTO;
        }

        
        public SavedAttachmentDTO InsertCommentAttachments(SavedAttachment CommentAttachment)
        {
            var Attachment = attachmentRepository.Insert(CommentAttachment);

            return _Mapper.Map(CommentAttachment, typeof(SavedAttachment), typeof(SavedAttachmentDTO)) as SavedAttachmentDTO;
        }

        public Survey InsertSurvey(Survey survey)
        {
            survey.CreatedByRole = _uow.GetRepository<CommiteeUsersRole>().GetAll().Where(x => x.UserId == survey.CreatedBy && x.CommiteeId == survey.CommiteeId).FirstOrDefault();
            
                survey.CreatedByRoleId = survey.CreatedByRole?.RoleId;
           //}
            
            var newSurvey = _uow.GetRepository<Survey>().Insert(survey);
            if (newSurvey.IsShared)
            {
                var committemebers = _UnitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.CommiteeId == newSurvey.CommiteeId && x.UserId != _sessionServices.UserId).ToList();
                foreach (var item in committemebers)
                {
                    var loc = _UnitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddNewSurveyNotificationText");
                    CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                    {
                        IsRead = false,
                        UserId = (int)item.UserId,
                        TextAR = loc.CommiteeLocalizationAr + " " + survey.Subject,
                        TextEn = loc.CommiteeLocalizationEn + " " + survey.Subject,
                        SurveyId = newSurvey.SurveyId,
                        CommiteeId = newSurvey.CommiteeId
                    };
                    List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                    _committeeNotificationService.Insert(committeeNotifications);
                }
            }
            else
            {
                foreach (var item in newSurvey.SurveyUsers)
                {
                    var loc = _UnitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddNewSurveyNotificationText");
                    if (item.UserId != _sessionServices.UserId)
                    {
                        CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                        {
                            IsRead = false,
                            UserId = (int)item.UserId,
                            TextAR = loc.CommiteeLocalizationAr + " " + survey.Subject,
                            TextEn = loc.CommiteeLocalizationEn + " " + survey.Subject,
                            SurveyId = newSurvey.SurveyId,
                            CommiteeId = newSurvey.CommiteeId
                        };
                        List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                        _committeeNotificationService.Insert(committeeNotifications);
                    }
                }
            }
           // newSurvey.CreatedByRole = _uow.GetRepository<CommiteeUsersRole>().GetAll().Where(x => x.UserId == newSurvey.CreatedBy && x.CommiteeId == newSurvey.CommiteeId).FirstOrDefault();
            //newSurvey.CreatedByRoleId = newSurvey.CreatedByRole.RoleId;
            return newSurvey;
        }

        public AttachmentSummaryDTO InsertAttachment(DocumentDTO entity, params string[] include)
        {
            //int x = 0;
            //UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId =
            //    _sessionServices.UserIdAndRoleIdAfterDecrypt(entity.SavedAttachmentId);

            //x = UserIdAndUserRoleId.Id;
            int TransactionTypeId = 0;
            int TransactionId = 0;
            if (include.Count() == 2)
            {
                int.TryParse(include[0], out TransactionTypeId);
                int.TryParse(include[1], out TransactionId);
            }
            IEnumerable<DocumentDTO> result;
            if (entity.BinaryContent != null && entity.BinaryContent.Length > 0)
            {
                Transaction _transaction = _UnitOfWork.GetRepository<Transaction>().GetById(TransactionId, true);
                var FileName = entity.AttachmentName ?? entity.OriginalName;
                entity.LFEntryId = _DocumentServices.CreateDocumentTransaction(1, TransactionId, TransactionTypeId, FileName, entity.MimeType, entity.BinaryContent, out int PagesCount, _transaction);
                entity.PagesCount = PagesCount;
                Attachment attachment = new Attachment()
                {
                    AttachmentId = entity.SavedAttachmentId,
                    AttachmentName = entity.AttachmentName,
                    AttachmentTypeId = entity.AttachmentTypeId,
                    Height = entity.Height,
                    LfentryId = entity.LFEntryId,
                    MimeType = entity.MimeType,
                    Notes = entity.Notes,
                    OriginalName = entity.OriginalName,
                    PagesCount = entity.PagesCount,
                    ReferenceAttachmentId = entity.ReferenceAttachmentId,
                    Size = entity.Size,
                    Width = entity.Width,
                };
                var Attch = _uow.GetRepository<Attachment>().Insert(attachment);
                //base.Insert(new List<DocumentDTO>() { entity });
                entity.BinaryContent = null;//to not returning the BinaryContent with the entity after it's been saved.
                entity.SavedAttachmentId = Attch.AttachmentId;
                result = new List<DocumentDTO>() { entity };
            }
            else
            {
                Attachment attachment = new Attachment()
                {
                    AttachmentId = entity.SavedAttachmentId,
                    AttachmentName = entity.AttachmentName,
                    AttachmentTypeId = entity.AttachmentTypeId,
                    Height = entity.Height,
                    LfentryId = entity.LFEntryId,
                    MimeType = entity.MimeType,
                    Notes = entity.Notes,
                    OriginalName = entity.OriginalName,
                    PagesCount = entity.PagesCount,
                    ReferenceAttachmentId = entity.ReferenceAttachmentId,
                    Size = entity.Size,
                    Width = entity.Width,
                };
                var Attch = _uow.GetRepository<Attachment>().Insert(attachment);
                //base.Insert(new List<DocumentDTO>() { entity });
                entity.BinaryContent = null;//to not returning the BinaryContent with the entity after it's been saved.
                entity.SavedAttachmentId = Attch.AttachmentId;
                result = new List<DocumentDTO>() { entity };
            }

            var attachmentSummaryDTO = base._Mapper.Map(result.First(), typeof(DocumentDTO), typeof(AttachmentSummaryDTO)) as AttachmentSummaryDTO;
            var shareAttachmentsSetting = this._SystemSettingsService.GetSystemSettingByCode("ShareAttachmentsByDefault");
            if (shareAttachmentsSetting != null)
            {
                attachmentSummaryDTO.IsShared = shareAttachmentsSetting.SystemSettingValue == "1" ? true : false;
            }
            return attachmentSummaryDTO;
        }
        public void SurveySignalR(UserChatDTO user, SurveyDTO survey)
        {
            _signalR.InsertSurveySender(user, survey, (int)_sessionServices.UserId);
        }
        public UserDetailsDTO GetUserById(int? createdBy)
        {
            return _UnitOfWork.GetRepository<User>().GetAll().Where(c => c.UserId == (int)createdBy)
                  .AsQueryable().ProjectTo<UserDetailsDTO>(_Mapper.ConfigurationProvider, _sessionServices).FirstOrDefault();
        }

        public Meeting GetMeetingById(int? meetingId)
        {
            return _UnitOfWork.GetRepository<Meeting>().Find(meetingId);
        }

        public Meeting GetMeetingByTopixId(int? meetingTopicId)
        {
            var meetingId = _uow.GetRepository<MeetingTopic>().Find(meetingTopicId).MeetingId;
            return _UnitOfWork.GetRepository<Meeting>().Find(meetingId);
        }

        public void AddNotificationForSurvey(int userId, int surveyId, string subject , int? MeetingId)
        {
            var loc = _uow.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddMOMSurveyNotificationText");
            CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
            {
                IsRead = false,
                UserId = userId,
                TextAR = loc.CommiteeLocalizationAr + " " + subject,
                TextEn = loc.CommiteeLocalizationEn + " " + subject,
                SurveyId = surveyId ,
                MeetingId = MeetingId
            };
            List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
            _committeeNotificationService.Insert(committeeNotifications);
        }
    }
}
