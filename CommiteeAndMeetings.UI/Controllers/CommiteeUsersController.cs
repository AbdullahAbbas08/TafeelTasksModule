using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.Enums;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using DbContexts.MasarContext.ProjectionModels;
using LinqHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.AcroFields;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommiteeUsersController : _BaseController<CommiteeMember, CommiteeMemberDTO>
    {
        private readonly ICommiteeUserService _commiteeMemberService;
        protected readonly IHelperServices.ISessionServices _SessionServices;
        private readonly ICommiteeRolePermissionService commiteeRolePermissionService;
        private readonly ICommiteeUserPermissionService commiteeUserPermissionService;

        public CommiteeUsersController(ICommiteeUserService businessService,
                                        IHelperServices.ISessionServices sessionSevices,

                                        ICommiteeUserPermissionService commiteeUserPermissionService, IDataProtectService _dataProtectService) : base(businessService, sessionSevices)
        {
            this._commiteeMemberService = businessService;
            _SessionServices = sessionSevices;
            this.commiteeUserPermissionService = commiteeUserPermissionService;
        }


        //[HttpPost("CommiteeUsersInsert")]
        //public virtual IEnumerable<CommiteeMemberDTO> CommiteeUsersPost([FromBody] IEnumerable<CommiteeMemberDTO> entities)
        //{
        //    if (entities.Count() > 0)
        //    {
        //        var entity = entities.FirstOrDefault();
        //        int RoleId = entity.CommiteeRoles.FirstOrDefault().RoleId;
        //        commiteeUserPermissionService.CustomInsert(RoleId, entity.UserId, entity.CommiteeIdEncrypt);
        //    }

        //    int commiteId = 0;
        //    foreach (var item in entities)
        //    {
        //        if (!string.IsNullOrEmpty(item.CommiteeIdEncrypt))
        //        {
        //            commiteId = _SessionServices.UserIdAndRoleIdAfterDecrypt(item.CommiteeIdEncrypt, true).Id;

        //        }
        //    }

        //    var CommiteeMember = this._commiteeMemberService.CheckIfUserExixt(commiteId, entities.First().UserId, entities.First().CommiteeRoles.First().RoleId);
        //    if (CommiteeMember == true)
        //        return new List<CommiteeMemberDTO>();
        //    IEnumerable<CommiteeMemberDTO> member = base.Post(entities);
        //    member = member.Select(c => new CommiteeMemberDTO
        //    {
        //        Active = c.Active,
        //        CommiteeId = c.CommiteeId,
        //        CommiteeMemberId = c.CommiteeMemberId,
        //        CommiteeRoles = c.CommiteeRoles.Select(x => new CommiteeUsersRoleDTO
        //        {
        //            CommiteeId = x.CommiteeId,
        //            CreatedUser = x.CreatedUser,
        //            Delegated = x.Delegated,
        //            Enabled = x.Enabled,
        //            EnableUntil = x.EnableUntil,
        //            Notes = x.Notes,
        //            RoleId = x.RoleId,
        //            UserId = x.UserId,
        //            CommiteeUsersRoleId = x.CommiteeUsersRoleId,
        //            RoleNameAR = _commiteeMemberService.GetRoleName(x.RoleId, "Ar"),
        //            RoleNameEn = _commiteeMemberService.GetRoleName(x.RoleId, "En"),

        //        }).ToList(),
        //        User = _commiteeMemberService.GetCommitteUser(c.UserId),
        //        UserId = c.UserId,
        //    });
        //    return member;
        //}

        [HttpGet("GetAllWithCounts")]
        public DataSourceResult<CommiteeMemberDTO> GetAllWithCounts([FromQuery] DataSourceRequest dataSourceRequest, string SearchName)
        {
            return _commiteeMemberService.GetAllWithCounts(dataSourceRequest, SearchName);
        }
        [HttpGet("GetAllByType/{External}")]
        public DataSourceResult<CommiteeMemberDTO> GetAllByType([FromQuery] DataSourceRequest dataSourceRequest, bool External)
        {
            return _commiteeMemberService.GetAllByType(dataSourceRequest, External);
        }
        [HttpGet("GetExternalUsers")]
        public List<VwLookUpReturnUser> GetExternalUsers(int take, int skip, string search)
        {
            return _commiteeMemberService.GetExternalUsers(take, skip, search);
        }
        [HttpGet("GetInternalUsers")]
        public List<VwLookUpReturnUser> GetInternalUsers(int take, int skip, string search)
        {
            return _commiteeMemberService.GetInternalUsers(take, skip, search);
        }
        [AllowAnonymous]
        [HttpPost("ConfirmChangeMemberState")]
        public bool ConfirmChangeMemberState(string commiteeMemberId, MemberState memberState)
        {
            return _commiteeMemberService.ConfirmChangeMemberState(commiteeMemberId, memberState);
        }
        [HttpGet("GetDeleatedUsers")]
        public List<LookUpDTO> GetDeleatedUsers(string CommitteeId)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteeId, true);

            return _commiteeMemberService.GetDeleatedUsers(UserIdAndUserRoleId.Id);
        }
        [HttpGet("GetRoles")]
        public List<LookUpDTO> GetRoles(int take, int skip)
        {
            return _commiteeMemberService.GetRoles(take, skip);
        }
        [HttpPost("ActiveDisactiveMember")]
        public virtual string ActiveDisactiveMember([FromBody] string ids, bool Active, MemberState memberState)
        {
            return this._commiteeMemberService.ActiveDisactiveMember(ids, Active, memberState);
        }
        [HttpGet("GetActiveUsersByCommitteeId")]
        public DataSourceResult<CommiteeMember> GetActiveUsersByCommitteeId(DataSourceRequest dataSourceRequest, string CommitteeId)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteeId, false);

            return _commiteeMemberService.GetActiveUsersByCommitteeId(dataSourceRequest, UserIdAndUserRoleId.Id);
        }
        public override IEnumerable<CommiteeMemberDTO> Post([FromBody] IEnumerable<CommiteeMemberDTO> entities)
        {
            int commiteId = 0;
            foreach (var item in entities)
            {
                if (!string.IsNullOrEmpty(item.CommiteeIdEncrypt))
                {
                    commiteId = _SessionServices.UserIdAndRoleIdAfterDecrypt(item.CommiteeIdEncrypt, true).Id;

                }
            }

            var CommiteeMember = this._commiteeMemberService.CheckIfUserExixt(commiteId, entities.First().UserId, entities.First().CommiteeRoles.First().RoleId);
            if (CommiteeMember == true)
                return new List<CommiteeMemberDTO>();
            var member = base.Post(entities);
            member = member.Select(c => new CommiteeMemberDTO
            {
                Active = c.Active,
                CommiteeId = c.CommiteeId,
                CommiteeMemberId = c.CommiteeMemberId,
                MemberState = c.MemberState,
                IsReserveMember = c.IsReserveMember,
                CommiteeRoles = c.CommiteeRoles.Select(x => new CommiteeUsersRoleDTO
                {
                    CommiteeId = x.CommiteeId,
                    CreatedUser = x.CreatedUser,
                    Delegated = x.Delegated,
                    Enabled = x.Enabled,
                    EnableUntil = x.EnableUntil,
                    Notes = x.Notes,
                    RoleId = x.RoleId,
                    UserId = x.UserId,
                    CommiteeUsersRoleId = x.CommiteeUsersRoleId,
                    RoleNameAR = _commiteeMemberService.GetRoleName(x.RoleId, "Ar"),
                    RoleNameEn = _commiteeMemberService.GetRoleName(x.RoleId, "En"),

                }).ToList(),
                User = _commiteeMemberService.GetCommitteUser(c.UserId),
                UserId = c.UserId,
            });
            return member;
        }
        [HttpGet("DelegateUser")]
        public CommiteeUsersRoleDTO DelegateUser(int userId, string committeId, int committeMemberId, string Note, DateTimeOffset? ToDate)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(committeId, true);
            commiteeUserPermissionService.DelegateUserAddPermissions(userId, committeId,true);
            return _commiteeMemberService.DelegateUser(userId, UserIdAndUserRoleId.Id, committeMemberId, Note, ToDate);
        }
        [HttpGet("DisableDelegateUser")]
        public bool DisableDelegateUser(int UserRoleID)
        {
            return _commiteeMemberService.DisableDelegateUser(UserRoleID);
        }
        [HttpGet("GetUserProfile")]
        public UserProfileDetailsDTO GetUserProfile()
        {
            return _commiteeMemberService.GetUserProfile();
        }
        [HttpPost("AddUserImage")]
        public object AddUserImage(int userId)
        {
            if (!Request.ContentType.StartsWith("multipart"))
            {
                throw new System.Exception("Invalid multipart request");
            }
            Microsoft.AspNetCore.Http.IFormFile file = Request.Form.Files[0];
            byte[] BinaryContent = null;
            using (System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(file.OpenReadStream()))
            {
                BinaryContent = binaryReader.ReadBytes((int)file.Length);
            }
            byte[] ProfileImage = BinaryContent;
            string ProfileImageMimeType = file.ContentType;
            var user = _commiteeMemberService.AddUserImage(userId, ProfileImage, ProfileImageMimeType);
            return new { UserId = user.UserIdEncrypt, ProfileImage = user.ProfileImage, ProfileImageMimeType = user.ProfileImageMimeType };
        }
        [HttpPost]
        [Route("ChangeUserPassword")]
        public async Task<ActionResult<bool>> ChangeUserPassword([FromBody] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = await _commiteeMemberService.GetCurrentUserAsync();
            if (user == null)
            {
                return BadRequest("NotFound");
            }
            (bool Succeeded, string Error) result = await _commiteeMemberService.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { res = true });
            }
            return Ok(new { res = false });
        }
    }
}