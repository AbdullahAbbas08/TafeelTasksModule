using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using HelperServices.LinqHelpers;
using IHelperServices.Models;
using LinqHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Models;
using System;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class SurveyService : BusinessService<Survey, SurveyDTO>, ISurveyService
    {
        IUnitOfWork _unitOfWork;
        IHelperServices.ISessionServices _sessionServices;
        public SurveyService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
        }
        public override DataSourceResult<SurveyDTO> GetAll<SurveyDTO>(DataSourceRequest dataSourceRequest, bool WithTracking = true)
        {
            IQueryable query = _unitOfWork.GetRepository<Survey>(false).GetAll(WithTracking).AsSingleQuery().Where(x => x.IsShared || x.SurveyUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId).OrderByDescending(x => x.CreatedOn);

            var res = query.ProjectTo<SurveyDTO>(_Mapper.ConfigurationProvider).ToDataSourceResult(dataSourceRequest);
            res.Data = res.Data.ToList();
            return res;
            


        }


        #region 
        // Api For Get All Survey
        public AllSurveyDTO GetAllServey(int take, int skip, int committeId, DateTime? dateFrom, DateTime? dateTo,string SearchText)
        {
            //Check For AllCommittePermission
            

            // check if userId is head of unit or member in committe
            var committeitem = _unitOfWork.GetRepository<Commitee>().GetAll().Where(x => x.CommiteeId == committeId).FirstOrDefault();
            var committeMember = _unitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.CommiteeId == committeId).ToList();
            var headOfUnitId = committeitem.CurrenHeadUnitId;
            
           if (headOfUnitId != _sessionServices.UserId && committeMember.All(x => x.UserId != _sessionServices.UserId))
            {
                var serveyAlls = new AllSurveyDTO();
                serveyAlls.Surveys = _unitOfWork.GetRepository<Survey>().GetAll().OrderByDescending(x => x.CreatedOn)
                                   .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
                                   //.Where(x => x.IsShared /*|| x.SurveyUsers.Any(z => z.UserId == _sessionServices.UserId)*/ || x.CreatedBy == _sessionServices.UserId)
                                   .Where(z => SearchText == "" || SearchText == null || z.Subject.Contains(SearchText))
                                   .Select(x => new SurveyDTO
                                   {
                                       Attachments = x.Attachments.Select(y => new SurveyAttachmentDTO
                                       {
                                           Attachment = new SavedAttachmentDTO
                                           {
                                               AttachmentName = y.Attachment.AttachmentName,
                                               AttachmentTypeId = y.Attachment.AttachmentTypeId,
                                               CreatedBy = y.Attachment.CreatedBy,
                                               CreatedByRole = y.Attachment.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                               {
                                                   CommiteeUsersRoleId = y.Attachment.CreatedByRole.CommiteeUsersRoleId,
                                                   Role = new CommiteeDetailsRoleDTO
                                                   {
                                                       CommiteeRoleId = y.Attachment.CreatedByRole.CommiteeUsersRoleId,
                                                       CommiteeRolesNameAr = y.Attachment.CreatedByRole.Role.CommiteeRolesNameAr,
                                                       CommiteeRolesNameEn = y.Attachment.CreatedByRole.Role.CommiteeRolesNameEn
                                                   }
                                               },
                                               CreatedByRoleId = y.Attachment.CreatedByRoleId,
                                               CreatedByUser = y.Attachment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                               {
                                                   UserId = y.Attachment.CreatedByUser.UserId,
                                                   UserName = y.Attachment.CreatedByUser.Username,
                                                   FullNameAr = y.Attachment.CreatedByUser.FullNameAr,
                                                   FullNameEn = y.Attachment.CreatedByUser.FullNameEn,
                                                   FullNameFn = y.Attachment.CreatedByUser.FullNameFn,
                                                   ProfileImage = y.Attachment.CreatedByUser.ProfileImage
                                               },
                                               Height = y.Attachment.Height,
                                               IsDisabled = y.Attachment.IsDisabled,
                                               LFEntryId = y.Attachment.LFEntryId,
                                               MimeType = y.Attachment.MimeType,
                                               Notes = y.Attachment.Notes,
                                               PagesCount = y.Attachment.PagesCount,
                                               SavedAttachmentId = y.Attachment.SavedAttachmentId,
                                               Size = y.Attachment.Size,
                                               Width = y.Attachment.Width
                                           },
                                           AttachmentId = y.AttachmentId,
                                           SurveyAttachmentId = y.SurveyAttachmentId,
                                           SurveyId = y.SurveyId
                                       }).ToList(),
                                       Comments = x.Comments.Select(y => new SurveyCommentDTO
                                       {
                                           Comment = new CommentDTO
                                           {
                                               CreatedOn = y.Comment.CreatedOn,
                                               CommentId = y.Comment.CommentId,
                                               Text = y.Comment.Text,
                                               CreatedByUser = y.Comment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                               {
                                                   UserId = y.Comment.CreatedByUser.UserId,
                                                   UserName = y.Comment.CreatedByUser.Username,
                                                   FullNameAr = y.Comment.CreatedByUser.FullNameAr,
                                                   FullNameEn = y.Comment.CreatedByUser.FullNameEn,
                                                   FullNameFn = y.Comment.CreatedByUser.FullNameFn,
                                                   ProfileImage = y.Comment.CreatedByUser.ProfileImage
                                               },
                                           },
                                           CommentId = y.CommentId,
                                           CreatedBy = y.CreatedBy,
                                           CreatedByRole = y.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                           {
                                               CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                               Role = new CommiteeDetailsRoleDTO
                                               {
                                                   CommiteeRoleId = y.CreatedByRole.Role.CommiteeRoleId,
                                                   CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                                   CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                               },
                                           },
                                           CreatedByRoleId = y.CreatedByRoleId,
                                           CreatedByUser = y.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                           {
                                               UserId = y.CreatedByUser.UserId,
                                               UserName = y.CreatedByUser.Username,
                                               FullNameAr = y.CreatedByUser.FullNameAr,
                                               FullNameEn = y.CreatedByUser.FullNameEn,
                                               FullNameFn = y.CreatedByUser.FullNameFn,
                                               ProfileImage = y.CreatedByUser.ProfileImage
                                           },
                                           CreatedOn = y.CreatedOn,
                                           SurveyCommentId = y.SurveyCommentId,
                                           SurveyId = y.SurveyId,

                                       }).ToList(),
                                       CommiteeId = x.CommiteeId,
                                       CreatedBy = x.CreatedBy,
                                       CreatedByRole = x.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                       {
                                           CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                           Role = new CommiteeDetailsRoleDTO
                                           {
                                               CommiteeRoleId = x.CreatedByRole.Role.CommiteeRoleId,
                                               CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                                               CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                                           },
                                       },
                                       CreatedByRoleId = x.CreatedByRoleId,
                                       CreatedByUser = x.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                       {
                                           UserId = x.CreatedByUser.UserId,
                                           UserName = x.CreatedByUser.Username,
                                           FullNameAr = x.CreatedByUser.FullNameAr,
                                           FullNameEn = x.CreatedByUser.FullNameEn,
                                           FullNameFn = x.CreatedByUser.FullNameFn,
                                           ProfileImage = x.CreatedByUser.ProfileImage
                                       },
                                       CreatedOn = x.CreatedOn,
                                       IsShared = x.IsShared,
                                       MeetingId = x.MeetingId,
                                       MeetingTopicId = x.MeetingTopicId,
                                       Multi = x.Multi,
                                       Subject = x.Subject,
                                       SurveyId = x.SurveyId,
                                       SurveyEndDate = x.SurveyEndDate,
                                       SurveyAnswers = x.SurveyAnswers.Select(y => new SurveyAnswerDTO
                                       {
                                           Answer = y.Answer,
                                           SurveyAnswerId = y.SurveyAnswerId,
                                           SurveyAnswerUsers = y.SurveyAnswerUsers.Select(z => new SurveyAnswerUserDTO
                                           {
                                               SurveyAnswerId = z.SurveyAnswerId,
                                               SurveyAnswerUserId = z.SurveyAnswerUserId,
                                               UserId = z.UserId,
                                               User = new UserDetailsDTO
                                               {
                                                   UserId = z.User.UserId,
                                                   UserName = z.User.Username,
                                                   FullNameAr = z.User.FullNameAr,
                                                   FullNameEn = z.User.FullNameEn,
                                                   FullNameFn = z.User.FullNameFn,
                                                   ProfileImage = z.User.ProfileImage
                                               }
                                           }).ToList(),
                                           SurveyId = y.SurveyId
                                       }).ToList(),
                                       SurveyUsers = x.SurveyUsers.Select(y => new SurveyUserDTO
                                       {
                                           SurveyId = y.SurveyId,
                                           UserId = y.UserId,
                                           SurveyUserId = y.SurveyUserId
                                       }).ToList()
                                   }).ToDataSource(take, skip);
                return serveyAlls;
            }
            var serveyAll = new AllSurveyDTO();
             serveyAll.Surveys = _unitOfWork.GetRepository<Survey>().GetAll().OrderByDescending(x => x.CreatedOn)
                                .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
                                .Where(x => x.IsShared || x.SurveyUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
                                .Where(z => SearchText == "" || SearchText == null || z.Subject.Contains(SearchText))
                                .Select(x => new SurveyDTO
                                   {
                                       Attachments = x.Attachments.Select(y => new SurveyAttachmentDTO
                                       {
                                           Attachment = new SavedAttachmentDTO
                                           {
                                               AttachmentName = y.Attachment.AttachmentName,
                                               AttachmentTypeId = y.Attachment.AttachmentTypeId,
                                               CreatedBy = y.Attachment.CreatedBy,
                                               CreatedByRole = y.Attachment.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                               {
                                                   CommiteeUsersRoleId = y.Attachment.CreatedByRole.CommiteeUsersRoleId,
                                                   Role = new CommiteeDetailsRoleDTO
                                                   {
                                                       CommiteeRoleId = y.Attachment.CreatedByRole.CommiteeUsersRoleId,
                                                       CommiteeRolesNameAr = y.Attachment.CreatedByRole.Role.CommiteeRolesNameAr,
                                                       CommiteeRolesNameEn = y.Attachment.CreatedByRole.Role.CommiteeRolesNameEn
                                                   }
                                               },
                                               CreatedByRoleId = y.Attachment.CreatedByRoleId,
                                               CreatedByUser = y.Attachment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                               {
                                                   UserId = y.Attachment.CreatedByUser.UserId,
                                                   UserName = y.Attachment.CreatedByUser.Username,
                                                   FullNameAr = y.Attachment.CreatedByUser.FullNameAr,
                                                   FullNameEn = y.Attachment.CreatedByUser.FullNameEn,
                                                   FullNameFn = y.Attachment.CreatedByUser.FullNameFn,
                                                   ProfileImage = y.Attachment.CreatedByUser.ProfileImage
                                               },
                                               Height = y.Attachment.Height,
                                               IsDisabled = y.Attachment.IsDisabled,
                                               LFEntryId = y.Attachment.LFEntryId,
                                               MimeType = y.Attachment.MimeType,
                                               Notes = y.Attachment.Notes,
                                               PagesCount = y.Attachment.PagesCount,
                                               SavedAttachmentId = y.Attachment.SavedAttachmentId,
                                               Size = y.Attachment.Size,
                                               Width = y.Attachment.Width
                                           },
                                           AttachmentId = y.AttachmentId,
                                           SurveyAttachmentId = y.SurveyAttachmentId,
                                           SurveyId = y.SurveyId
                                       }).ToList(),
                                       Comments = x.Comments.Select(y => new SurveyCommentDTO
                                       {
                                           Comment = new CommentDTO
                                           {
                                               CreatedOn = y.Comment.CreatedOn,
                                               CommentId = y.Comment.CommentId,
                                               Text = y.Comment.Text,
                                               CreatedByUser = y.Comment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                               {
                                                   UserId = y.Comment.CreatedByUser.UserId,
                                                   UserName = y.Comment.CreatedByUser.Username,
                                                   FullNameAr = y.Comment.CreatedByUser.FullNameAr,
                                                   FullNameEn = y.Comment.CreatedByUser.FullNameEn,
                                                   FullNameFn = y.Comment.CreatedByUser.FullNameFn,
                                                   ProfileImage = y.Comment.CreatedByUser.ProfileImage
                                               },
                                           },
                                           CommentId = y.CommentId,
                                           CreatedBy = y.CreatedBy,
                                           CreatedByRole = y.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                           {
                                               CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                               Role = new CommiteeDetailsRoleDTO
                                               {
                                                   CommiteeRoleId = y.CreatedByRole.Role.CommiteeRoleId,
                                                   CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                                   CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                               },
                                           },
                                           CreatedByRoleId = y.CreatedByRoleId,
                                           CreatedByUser = y.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                           {
                                               UserId = y.CreatedByUser.UserId,
                                               UserName = y.CreatedByUser.Username,
                                               FullNameAr = y.CreatedByUser.FullNameAr,
                                               FullNameEn = y.CreatedByUser.FullNameEn,
                                               FullNameFn = y.CreatedByUser.FullNameFn,
                                               ProfileImage = y.CreatedByUser.ProfileImage
                                           },
                                           CreatedOn = y.CreatedOn,
                                           SurveyCommentId = y.SurveyCommentId,
                                           SurveyId = y.SurveyId,

                                       }).ToList(),
                                       CommiteeId = x.CommiteeId,
                                       CreatedBy = x.CreatedBy,
                                       CreatedByRole = x.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                       {
                                           CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                           Role = new CommiteeDetailsRoleDTO
                                           {
                                               CommiteeRoleId = x.CreatedByRole.Role.CommiteeRoleId,
                                               CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                                               CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                                           },
                                       },
                                       CreatedByRoleId = x.CreatedByRoleId,
                                       CreatedByUser = x.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                       {
                                           UserId = x.CreatedByUser.UserId,
                                           UserName = x.CreatedByUser.Username,
                                           FullNameAr = x.CreatedByUser.FullNameAr,
                                           FullNameEn = x.CreatedByUser.FullNameEn,
                                           FullNameFn = x.CreatedByUser.FullNameFn,
                                           ProfileImage = x.CreatedByUser.ProfileImage
                                       },
                                       CreatedOn = x.CreatedOn,
                                       IsShared = x.IsShared,
                                       MeetingId = x.MeetingId,
                                       MeetingTopicId = x.MeetingTopicId,
                                       Multi = x.Multi,
                                       Subject = x.Subject,
                                       SurveyId = x.SurveyId,
                                       SurveyEndDate = x.SurveyEndDate,
                                       SurveyAnswers = x.SurveyAnswers.Select(y => new SurveyAnswerDTO
                                       {
                                           Answer = y.Answer,
                                           SurveyAnswerId = y.SurveyAnswerId,
                                           SurveyAnswerUsers = y.SurveyAnswerUsers.Select(z => new SurveyAnswerUserDTO
                                           {
                                               SurveyAnswerId = z.SurveyAnswerId,
                                               SurveyAnswerUserId = z.SurveyAnswerUserId,
                                               UserId = z.UserId,
                                               User = new UserDetailsDTO
                                               {
                                                   UserId = z.User.UserId,
                                                   UserName = z.User.Username,
                                                   FullNameAr = z.User.FullNameAr,
                                                   FullNameEn = z.User.FullNameEn,
                                                   FullNameFn = z.User.FullNameFn,
                                                   ProfileImage = z.User.ProfileImage
                                               }
                                           }).ToList(),
                                           SurveyId = y.SurveyId
                                       }).ToList(),
                                       SurveyUsers = x.SurveyUsers.Select(y => new SurveyUserDTO
                                       {
                                           SurveyId = y.SurveyId,
                                           UserId = y.UserId,
                                           SurveyUserId = y.SurveyUserId
                                       }).ToList()
                                   }).ToDataSource(take,skip);
            return serveyAll;
        }
        #endregion
        //public IEnumerable<SurveyDTO> InsertWithAttachment(IEnumerable<SurveyDTO> entities)
        //{
        //    var ser= this._UnitOfWork.GetRepository<SurveyDTO>().Insert(entities);
        //    string pdfResolution = _systemSettingsService.GetSystemSettingByCode("PDFResolution").SystemSettingValue;
        //    Boolean pdfToColor = Convert.ToBoolean(_systemSettingsService.GetSystemSettingByCode("PDFToColor").SystemSettingValue);
        //    string AddIndexToAttachment = _systemSettingsService.GetSystemSettingByCode("AddIndexToAttachment").SystemSettingValue;

        //    if (!Request.ContentType.StartsWith("multipart"))
        //    {
        //        throw new System.Exception("Invalid multipart request");
        //    }
        //    // Get Doc Setting For Office 
        //    var OfficeSetting = this._AppSettings.DocumentSettings.OfficeSetting;
        //    bool ActiveOfficeConvertor = false;
        //    if (OfficeSetting != null)
        //    {
        //        if (OfficeSetting == "1") ActiveOfficeConvertor = true; else ActiveOfficeConvertor = false;
        //    }
        //    #region Get Additional Parameters
        //    var xx = Request.Form;
        //    var Description = Request.Form["description"].ToString();
        //    bool allUser = Convert.ToBoolean(Request.Form["allUsers"].ToString());
        //    var selectedUsers = Request.Form["selectedUsers"].ToString();
        //    var selcteduser = selectedUsers.Split(",");
        //    List<AttachmentUser> users = new List<AttachmentUser>();
        //    if (!allUser && selectedUsers != string.Empty && selectedUsers != "undefined")
        //    {
        //        foreach (var item in selcteduser)
        //        {
        //            users.Add(new AttachmentUser { UserId = Convert.ToInt32(item) });
        //        }
        //    }

        //    var AdditionalParameters = Request.Query.Union(Request.Form);
        //    string FileName = AdditionalParameters.Where(x => x.Key == "FileName").Select(x => x.Value).FirstOrDefault();
        //    string ContentType = AdditionalParameters.Where(x => x.Key == "ContentType").Select(x => x.Value).FirstOrDefault();

        //    int AttachmentTypeId;
        //    int.TryParse(AdditionalParameters.FirstOrDefault(x => x.Key == "AttachmentTypeId").Value, out AttachmentTypeId);

        //    int SurveyId;
        //    int.TryParse(AdditionalParameters.FirstOrDefault(x => x.Key == "SurveyId").Value, out SurveyId);

        //    #endregion Get Additional Parameters
        //    var files = Request.Form.Files;
        //    var attachmentList = new List<SavedAttachmentDTO>();
        //    var CommitteAttachmentList = new List<SurveyAttachmentDTO>();
        //    int i = 1;
        //    foreach (var file in files)
        //    {
        //        byte[] BinaryContent = null;
        //        byte[] OfficeBinaryContent = null;

        //        using (var binaryReader = new System.IO.BinaryReader(file.OpenReadStream()))
        //        {
        //            BinaryContent = binaryReader.ReadBytes((int)file.Length);
        //        }


        //        if ((Path.GetExtension(file.FileName).ToLower() == ".docx".ToLower() || Path.GetExtension(file.FileName).ToLower() == ".doc".ToLower()) && ActiveOfficeConvertor)
        //        {
        //            GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
        //            OfficeBinaryContent = BinaryContent;
        //            var tiff_path = ghostScriptFactory.ConvertWordToTIFF(BinaryContent, Path.GetExtension(file.FileName));
        //            BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
        //            ContentType = "image/tiff";
        //            try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
        //        }
        //        else if ((Path.GetExtension(file.FileName).ToLower() == ".xlsx".ToLower() || Path.GetExtension(file.FileName).ToLower() == ".xls".ToLower()) && ActiveOfficeConvertor)
        //        {
        //            GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
        //            OfficeBinaryContent = BinaryContent;
        //            var tiff_path = ghostScriptFactory.ConverExcelToTIFF(BinaryContent, (Path.GetExtension(file.FileName)));
        //            BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
        //            ContentType = "image/tiff";
        //            try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
        //        }
        //        else if (Path.GetExtension(file.FileName).ToLower() == ".pdf".ToLower())
        //        {
        //            //convert html file to tiff
        //            GhostFactory.GhostScriptFactory ghostScriptFactory = new GhostFactory.GhostScriptFactory(Hosting.RootPath, Hosting.AngularRootPath, pdfResolution, pdfToColor);
        //            var tiff_path = ghostScriptFactory.ConvertPDFtoTIFF(BinaryContent);
        //            BinaryContent = System.IO.File.ReadAllBytes(tiff_path);
        //            ContentType = "image/tiff";
        //            //try { Task.Run(() => System.IO.File.Delete(tiff_path)); } catch { }
        //        }


        //        var attachmentEntity = new DocumentDTO()
        //        {
        //            AttachmentTypeId = AttachmentTypeId == 0 ? 1 : AttachmentTypeId,
        //            AttachmentName = FileName ?? file.FileName.Split('\\').Last(),
        //            OriginalName = FileName ?? file.FileName.Split('\\').Last(),
        //            MimeType = ContentType ?? file.ContentType,
        //            Size = BinaryContent.Length,
        //            //  BinaryContent = BinaryContent
        //        };
        //        string[] param = new string[]
        //        {
        //            SurveyId.ToString()
        //        };
        //        attachmentList.Add(_IDocumentServices.Insert(attachmentEntity, param));
        //        CommitteAttachmentList.Add(_IDocumentServices.InsertSurveyAttachment(new SurveyAttachment
        //        {
        //            Active = true,
        //            AttachmentId = attachmentList.First().SavedAttachmentId,
        //            SurveyId = SurveyId
        //        }));
        //        ContentType = null;
        //        if (OfficeBinaryContent != null && this._AppSettings.DocumentSettings.ECMType.Equals("1"))
        //        {
        //            _IDocumentServices.InsertElecDoc(int.Parse(attachmentEntity.LFEntryId), OfficeBinaryContent, Path.GetExtension(file.FileName));
        //        }
        //        i++;
        //    }
        //    return CommitteAttachmentList;
        //}
    }
}