using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using HelperServices.LinqHelpers;
using IHelperServices.Models;
using LinqHelper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Models;
using System;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class CommiteeAttachmentService : BusinessService<CommiteeSavedAttachment, CommiteeAttachmentDTO>, ICommiteeAttachmentService
    {
        IUnitOfWork _unitOfWork;
        IHelperServices.ISessionServices _sessionServices;
        public CommiteeAttachmentService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
        }
        public override DataSourceResult<CommiteeAttachmentDTO> GetAll<CommiteeAttachmentDTO>(DataSourceRequest dataSourceRequest, bool WithTracking = true)
        {
            IQueryable query = _unitOfWork.GetRepository<CommiteeSavedAttachment>(false).GetAll(WithTracking).OrderByDescending(x => x.CreatedOn).Where(x => x.AllUsers || x.AttachmentUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId);

            return query.ProjectTo<CommiteeAttachmentDTO>(_Mapper.ConfigurationProvider).ToDataSourceResult(dataSourceRequest);
        }

        //Api For Get All Attchments
        public AllCommiteeAttachmentDTO GetAllCommiteeAttachment(int take, int skip, int committeId, DateTime? dateFrom, DateTime? dateTo, string SearchText)
        {
            // check if userId is head of unit or member in committe
            var committeitem = _unitOfWork.GetRepository<Commitee>().GetAll().Where(x => x.CommiteeId == committeId).FirstOrDefault();
            var committeMember = _unitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.CommiteeId == committeId).ToList();
            var headOfUnitId = committeitem.CurrenHeadUnitId;
            var AttchmentAll = new AllCommiteeAttachmentDTO();
            if (headOfUnitId != _sessionServices.UserId && committeMember.All(x => x.UserId != _sessionServices.UserId))
            {
                 AttchmentAll = new AllCommiteeAttachmentDTO();
                AttchmentAll.Attachments = _unitOfWork.GetRepository<CommiteeSavedAttachment>().GetAll().OrderByDescending(x => x.CreatedOn)
                             .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
                             //.Where(x => x.AllUsers || x.AttachmentUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
                             .Where(z => SearchText == "" || SearchText == null || z.Description.Contains(SearchText))
                             .Select(x => new CommiteeAttachmentDTO
                             {
                                 AllUsers = x.AllUsers,
                                 AttachmentComments = x.AttachmentComments.Select(y => new AttachmentCommentDTO
                                 {
                                     AttachmentCommentId = y.AttachmentCommentId,
                                     AttachmentId = y.AttachmentId,
                                     Comment = new CommentDTO
                                     {
                                         CommentId = y.CommentId,
                                         Text = y.Comment.Text,
                                         CreatedByUser = y.Comment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                         {
                                             UserId = y.CreatedByUser.UserId,
                                             UserName = y.CreatedByUser.Username,
                                             FullNameAr = y.CreatedByUser.FullNameAr,
                                             FullNameEn = y.CreatedByUser.FullNameEn,
                                             FullNameFn = y.CreatedByUser.FullNameFn,
                                             ProfileImage = y.CreatedByUser.ProfileImage
                                         },
                                     },
                                     CommentId = y.CommentId,
                                     CreatedBy = y.CreatedBy,
                                     CreatedByRoleId = y.CreatedByRoleId,
                                     CreatedByRole = y.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                     {
                                         CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                         Role = new CommiteeDetailsRoleDTO
                                         {
                                             CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                             CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                             CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                         }
                                     },
                                     CreatedByUser = new UserDetailsDTO
                                     {
                                         UserId = y.CreatedByUser.UserId,
                                         UserName = y.CreatedByUser.Username,
                                         FullNameAr = y.CreatedByUser.FullNameAr,
                                         FullNameEn = y.CreatedByUser.FullNameEn,
                                         FullNameFn = y.CreatedByUser.FullNameFn,
                                         ProfileImage = y.CreatedByUser.ProfileImage
                                     },
                                     CreatedOn = y.CreatedOn,

                                 }).ToList(),
                                 CommiteeAttachmentId = x.CommiteeAttachmentId,
                                 CommiteeId = x.CommiteeId,
                                 CreatedBy = x.CreatedBy,
                                 CreatedOn = x.CreatedOn,
                                 Description = x.Description,
                                 Attachments = x.Attachments.Select(y => new SavedAttachmentDTO
                                 {
                                     AttachmentName = y.AttachmentName,
                                     AttachmentTypeId = y.AttachmentTypeId,
                                     CreatedBy = y.CreatedBy,
                                     CreatedByRole = y.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                     {
                                         CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                         Role = new CommiteeDetailsRoleDTO
                                         {
                                             CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                             CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                             CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                         }
                                     },
                                     CreatedByRoleId = y.CreatedByRoleId,
                                     CreatedByUser = y.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                                     {
                                         UserId = y.CreatedByUser.UserId,
                                         UserName = y.CreatedByUser.Username,
                                         FullNameAr = y.CreatedByUser.FullNameAr,
                                         FullNameEn = y.CreatedByUser.FullNameEn,
                                         FullNameFn = y.CreatedByUser.FullNameFn,
                                         ProfileImage = y.CreatedByUser.ProfileImage
                                     },
                                     Height = y.Height,
                                     IsDisabled = y.IsDisabled,
                                     LFEntryId = y.LFEntryId,
                                     MimeType = y.MimeType,
                                     Notes = y.Notes,
                                     PagesCount = y.PagesCount,
                                     SavedAttachmentId = y.SavedAttachmentId,
                                     Size = y.Size,
                                     Width = y.Width

                                 }).ToList(),
                                 AttachmentUsers = x.AttachmentUsers.Select(y => new AttachmentUserDTO
                                 {
                                     AttachmentUserId = y.AttachmentUserId,
                                     CommiteeAttachmentId = y.CommiteeAttachmentId,
                                     User = new UserDetailsDTO
                                     {
                                         UserId = y.User.UserId,
                                         UserName = y.User.Username,
                                         FullNameAr = y.User.FullNameAr,
                                         FullNameEn = y.User.FullNameEn,
                                         FullNameFn = y.User.FullNameFn,
                                         ProfileImage = y.User.ProfileImage
                                     },
                                     UserId = y.UserId
                                 }).ToList(),
                                 CreatedByRole = x.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                 {
                                     CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                     Role = new CommiteeDetailsRoleDTO
                                     {
                                         CommiteeRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                         CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                                         CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                                     }
                                 },
                                 CreatedByRoleId = x.CreatedByRoleId,
                                 CreatedByUser = x.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                                 {
                                     UserId = x.CreatedByUser.UserId,
                                     UserName = x.CreatedByUser.Username,
                                     FullNameAr = x.CreatedByUser.FullNameAr,
                                     FullNameEn = x.CreatedByUser.FullNameEn,
                                     FullNameFn = x.CreatedByUser.FullNameFn,
                                     ProfileImage = x.CreatedByUser.ProfileImage
                                 }
                             }).ToDataSource(take, skip);

                return AttchmentAll;
            }
                 AttchmentAll = new AllCommiteeAttachmentDTO();
            AttchmentAll.Attachments = _unitOfWork.GetRepository<CommiteeSavedAttachment>().GetAll().OrderByDescending(x => x.CreatedOn)
                         .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
                         .Where(x => x.AllUsers || x.AttachmentUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
                         .Where(z => SearchText == "" || SearchText == null || z.Description.Contains(SearchText))
                         .Select(x => new CommiteeAttachmentDTO
                         {
                             AllUsers = x.AllUsers,
                             AttachmentComments = x.AttachmentComments.Select(y => new AttachmentCommentDTO
                             {
                                 AttachmentCommentId = y.AttachmentCommentId,
                                 AttachmentId = y.AttachmentId,
                                 Comment = new CommentDTO
                                 {
                                     CommentId = y.CommentId,
                                     Text = y.Comment.Text,
                                     CreatedByUser = y.Comment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                     {
                                         UserId = y.CreatedByUser.UserId,
                                         UserName = y.CreatedByUser.Username,
                                         FullNameAr = y.CreatedByUser.FullNameAr,
                                         FullNameEn = y.CreatedByUser.FullNameEn,
                                         FullNameFn = y.CreatedByUser.FullNameFn,
                                         ProfileImage = y.CreatedByUser.ProfileImage
                                     },
                                 },
                                 CommentId = y.CommentId,
                                 CreatedBy = y.CreatedBy,
                                 CreatedByRoleId = y.CreatedByRoleId,
                                 CreatedByRole = y.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                 {
                                     CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                     Role = new CommiteeDetailsRoleDTO
                                     {
                                         CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                         CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                         CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                     }
                                 },
                                 CreatedByUser = new UserDetailsDTO
                                 {
                                     UserId = y.CreatedByUser.UserId,
                                     UserName = y.CreatedByUser.Username,
                                     FullNameAr = y.CreatedByUser.FullNameAr,
                                     FullNameEn = y.CreatedByUser.FullNameEn,
                                     FullNameFn = y.CreatedByUser.FullNameFn,
                                     ProfileImage = y.CreatedByUser.ProfileImage
                                 },
                                 CreatedOn = y.CreatedOn,

                             }).ToList(),
                             CommiteeAttachmentId = x.CommiteeAttachmentId,
                             CommiteeId = x.CommiteeId,
                             CreatedBy = x.CreatedBy,
                             CreatedOn = x.CreatedOn,
                             Description = x.Description,
                             Attachments = x.Attachments.Select(y => new SavedAttachmentDTO
                             {
                                 AttachmentName = y.AttachmentName,
                                 AttachmentTypeId = y.AttachmentTypeId,
                                 CreatedBy = y.CreatedBy,
                                 CreatedByRole = y.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                 {
                                     CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                     Role = new CommiteeDetailsRoleDTO
                                     {
                                         CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                         CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                         CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                     }
                                 },
                                 CreatedByRoleId = y.CreatedByRoleId,
                                 CreatedByUser = y.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                                 {
                                     UserId = y.CreatedByUser.UserId,
                                     UserName = y.CreatedByUser.Username,
                                     FullNameAr = y.CreatedByUser.FullNameAr,
                                     FullNameEn = y.CreatedByUser.FullNameEn,
                                     FullNameFn = y.CreatedByUser.FullNameFn,
                                     ProfileImage = y.CreatedByUser.ProfileImage
                                 },
                                 Height = y.Height,
                                 IsDisabled = y.IsDisabled,
                                 LFEntryId = y.LFEntryId,
                                 MimeType = y.MimeType,
                                 Notes = y.Notes,
                                 PagesCount = y.PagesCount,
                                 SavedAttachmentId = y.SavedAttachmentId,
                                 Size = y.Size,
                                 Width = y.Width

                             }).ToList(),
                             AttachmentUsers = x.AttachmentUsers.Select(y => new AttachmentUserDTO
                             {
                                 AttachmentUserId = y.AttachmentUserId,
                                 CommiteeAttachmentId = y.CommiteeAttachmentId,
                                 User = new UserDetailsDTO
                                 {
                                     UserId = y.User.UserId,
                                     UserName = y.User.Username,
                                     FullNameAr = y.User.FullNameAr,
                                     FullNameEn = y.User.FullNameEn,
                                     FullNameFn = y.User.FullNameFn,
                                     ProfileImage = y.User.ProfileImage
                                 },
                                 UserId = y.UserId
                             }).ToList(),
                             CreatedByRole = x.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                             {
                                 CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                 Role = new CommiteeDetailsRoleDTO
                                 {
                                     CommiteeRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                     CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                                     CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                                 }
                             },
                             CreatedByRoleId = x.CreatedByRoleId,
                             CreatedByUser = x.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                             {
                                 UserId = x.CreatedByUser.UserId,
                                 UserName = x.CreatedByUser.Username,
                                 FullNameAr = x.CreatedByUser.FullNameAr,
                                 FullNameEn = x.CreatedByUser.FullNameEn,
                                 FullNameFn = x.CreatedByUser.FullNameFn,
                                 ProfileImage = x.CreatedByUser.ProfileImage
                             }
                         }).ToDataSource(take, skip);

            return AttchmentAll;

        }


    }
}
