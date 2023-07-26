using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommiteeUserPermissionsController : _BaseController<CommiteeUsersPermission, CommiteeUserPermissionDTO>
    {
        private readonly ICommiteeUserPermissionService commiteUserPermissionService;
        private readonly IUnitOfWork unitOfWork;
        public CommiteeUserPermissionsController(ICommiteeUserPermissionService _commiteeUserPermissionService,
                                                 IHelperServices.ISessionServices sessionSevices,
                                                 IUnitOfWork _unitOfWork) : base(_commiteeUserPermissionService, sessionSevices)
        {
            commiteUserPermissionService = _commiteeUserPermissionService;
            unitOfWork = _unitOfWork;
        }


        [HttpPut("UpdateCustome")]
        public virtual IEnumerable<CommiteeUserPermissionDTO> PutCustome([FromBody] IEnumerable<CommiteeUserPermissionDTO> entities)
        {
            foreach (var item in entities)
            {
                var PermissionExist = unitOfWork.GetRepository<CommiteeUsersPermission>()
                                                    .GetAll()
                                                    .Where(x => x.RoleId == item.RoleId &&
                                                           x.CommiteeId == item.CommiteeId &&
                                                           x.UserId == item.UserId &&
                                                           x.PermissionId == item.PermissionId).ToList();
                if (PermissionExist.Count() >0) item.Commitee_CommiteePermissionId = PermissionExist.FirstOrDefault().Commitee_CommiteePermissionId;
            }
            return this._BusinessService.Update(entities);
        }

        //[HttpPost("CommiteeUsersGetAll")]
        //public virtual IEnumerable<CommiteeUserPermissionDTO> CommiteeUsersGetAll(bool IsDelegated, int UserId, int CommiteeId)
        //{
        //    IQueryable<CommitePermission> permissions = unitOfWork.GetRepository<CommitePermission>().GetAll();
        //    IQueryable<CommiteeUsersPermission> UserPermission = unitOfWork.GetRepository<CommiteeUsersPermission>().GetAll().Where(x => x.IsDelegated == IsDelegated && x.UserId == UserId && x.CommiteeId == CommiteeId);
        //    var result = from P in permissions
        //                 join UP in UserPermission
        //                 on P.CommitePermissionId equals UP.PermissionId
        //                 select (new CommiteeUserPermissionDTO
        //                 {
        //                     Commitee_CommiteePermissionId = UP.Commitee_CommiteePermissionId,
        //                     CommiteeUserPermissionId = UP.PermissionId,
        //                     CommiteeId = UP.CommiteeId,
        //                     PermissionId = UP.PermissionId,
        //                     CommitePermissionNameAr = P.CommitePermissionNameAr,
        //                     CommitePermissionNameEn = P.CommitePermissionNameEn,
        //                     CommitePermissionNameFn = P.CommitePermissionNameFn,
        //                     Enabled = UP.Enabled,
        //                     Notes = UP.Notes,
        //                     UserId = UP.UserId,
        //                 });
        //    List<CommiteeUserPermissionDTO> PermissionsResult = new();
        //    foreach (var item in permissions)
        //    {
        //        var Exist = result.FirstOrDefault(x => x.PermissionId == item.CommitePermissionId);
        //        if (Exist !=null)
        //        {
        //            PermissionsResult.Add(new CommiteeUserPermissionDTO
        //            {
        //                Commitee_CommiteePermissionId = Exist.Commitee_CommiteePermissionId,
        //                CommiteeUserPermissionId=Exist.CommiteeUserPermissionId,
        //                CommiteeId = Exist.CommiteeId,
        //                PermissionId = item.CommitePermissionId,
        //                CommitePermissionNameAr = item.CommitePermissionNameAr,
        //                CommitePermissionNameEn = item.CommitePermissionNameEn,
        //                CommitePermissionNameFn = item.CommitePermissionNameFn,
        //                Enabled = Exist.Enabled,
        //                Notes = Exist.Notes,
        //                UserId = Exist.UserId,
        //            });
        //        }
        //        else
        //        {
        //            PermissionsResult.Add(new CommiteeUserPermissionDTO
        //            {
        //                CommiteeId = 0,
        //                PermissionId = item.CommitePermissionId,
        //                CommitePermissionNameAr = item.CommitePermissionNameAr,
        //                CommitePermissionNameEn = item.CommitePermissionNameEn,
        //                CommitePermissionNameFn = item.CommitePermissionNameFn,
        //                Enabled = false,
        //                Notes = "",
        //                UserId = 0,
        //            });
        //        }
        //    }

        //    return PermissionsResult;

        //}
    }
}
