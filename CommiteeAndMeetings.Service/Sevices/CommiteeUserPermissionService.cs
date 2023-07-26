using AutoMapper;
using CommiteeAndMeetings.BLL;
using CommiteeAndMeetings.BLL.BaseObjects;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class CommiteeUserPermissionService : BusinessService<CommiteeUsersPermission, CommiteeUserPermissionDTO>, ICommiteeUserPermissionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHelperServices.ISessionServices sessionServices;
        private readonly ICommiteeRolePermissionService commiteeRolePermissionService;

        public CommiteeUserPermissionService(IUnitOfWork unitOfWork,
            IMapper mapper, 
            IStringLocalizer stringLocalizer,
            ISecurityService securityService, 
            IHelperServices.ISessionServices sessionServices,
            IOptions<AppSettings> appSettings,
            ICommiteeRolePermissionService _commiteeRolePermissionService )
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            this.unitOfWork = unitOfWork;
            this.sessionServices = sessionServices;
            commiteeRolePermissionService = _commiteeRolePermissionService;
        }

        public void CustomInsert(int RoleId, int UserId , string CommiteeIdEncrypt, bool IsDelegated = false)
        {
           
           
            List<CommiteeRolePermission> RolePermissions = commiteeRolePermissionService.GetAllPermission().Where(x => x.RoleId == RoleId).ToList();
            List<CommiteeUserPermissionDTO> commiteeUserPermissionDTO = new();
            foreach (var item in RolePermissions)
            {
                commiteeUserPermissionDTO.Add(new CommiteeUserPermissionDTO
                {
                    RoleId = RoleId,
                    PermissionId = item.PermissionId,
                    UserId = UserId,
                    Enabled = item.Permission.Enabled,
                    CommiteeId = sessionServices.UserIdAndRoleIdAfterDecrypt(CommiteeIdEncrypt, true).Id,
                    IsDelegated = IsDelegated

                });
            }
            if (commiteeUserPermissionDTO.Count() > 0) Insert(commiteeUserPermissionDTO);   
        }

        public void DelegateUserAddPermissions(int userId, string committeId, bool IsDelegated = false)
        {
            int RoleId = unitOfWork.GetRepository<CommiteeRole>().GetAll().Where(c => c.IsMangerRole).FirstOrDefault().CommiteeRoleId;
            CustomInsert(RoleId, userId, committeId, IsDelegated);
        }
    }
}