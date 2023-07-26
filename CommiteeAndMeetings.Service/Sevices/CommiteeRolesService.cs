using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
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
    public class CommiteeRolesService : BusinessService<DAL.CommiteeDomains.CommiteeRole, CommiteeRoleDTO>, ICommiteeRolesService
    {
        private IUnitOfWork _unitOfWork;
        public CommiteeRolesService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
        }
        public override IEnumerable<CommiteeRoleDTO> Update(IEnumerable<CommiteeRoleDTO> Entities)
        {
            foreach (var CommiteeRole in Entities)
            {
                var RolePermission = _unitOfWork.GetRepository<CommiteeRolePermission>().GetAll().Where(x => x.RoleId == CommiteeRole.CommiteeRoleId).ToList();
                if (RolePermission.Count() != 0)
                {
                    _unitOfWork.GetRepository<CommiteeRolePermission>().Delete(RolePermission);
                    _unitOfWork.SaveChanges();
                }
                //  make IsManagerRole equal false for other Committe Role  if this CommitteRole equal true 
                if (CommiteeRole.IsMangerRole)
                {
                    
                    var CommitteRoles = _unitOfWork.GetRepository<CommiteeRole>().GetAll().Where(x => x.IsMangerRole).ToList();
                    foreach (var item in CommitteRoles)
                    {
                        item.IsMangerRole = false;
                        _unitOfWork.SaveChanges();
                    }
                }
            }
            return base.Update(Entities);
        }
        public override IEnumerable<CommiteeRoleDTO> Insert(IEnumerable<CommiteeRoleDTO> entities)
        {
            if (entities.First().IsMangerRole)
            {
                var MangerRole = _unitOfWork.GetRepository<CommiteeRole>().GetAll().Where(x => x.IsMangerRole).Count();
                return MangerRole > 0 ? null : base.Insert(entities);
            }
            else
            {
                return base.Insert(entities);
            }
        }
    }
}
