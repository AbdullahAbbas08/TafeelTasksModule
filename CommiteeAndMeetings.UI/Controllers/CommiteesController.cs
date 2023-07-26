using CommiteeAndMeetings.BLL;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Service.Sevices;
using CommiteeDatabase.Models.Domains;
using HelperServices;
using IHelperServices.Models;
using LinqHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommiteesController : _BaseController<Commitee, CommiteeDTO>
    {
        private readonly ICommiteeService _commiteeService;
        public AppSettings _AppSettings;
        protected readonly IHelperServices.ISessionServices _SessionServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommiteeUserPermissionService commiteeUserPermissionService;
        private readonly ICommiteeRolePermissionService commiteeRolePermissionService;

        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public CommiteesController(ICommiteeService businessService,
            IHelperServices.ISessionServices sessionSevices,
            IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor,
             ICommiteeUserPermissionService commiteeUserPermissionService,
              ICommiteeRolePermissionService commiteeRolePermissionService ) : base(businessService, sessionSevices)
        {
            this._commiteeService = businessService;
            _AppSettings = new AppSettings();
            _AppSettings = appSettings.Value;
            _SessionServices = sessionSevices;
            _httpContextAccessor = httpContextAccessor;
            this.commiteeUserPermissionService = commiteeUserPermissionService;
            this.commiteeRolePermissionService = commiteeRolePermissionService;
        }


        //[HttpPost("CustomInsert")]
        //public virtual IEnumerable<CommiteeDTO> CustomPost([FromBody] IEnumerable<CommiteeDTO> entities)
        //{
        //    var commitee = _BusinessService.Insert(entities);
        //    var commitee1 = commitee.FirstOrDefault();
        //    if (commitee1 != null)
        //    {
        //        int RoleId = 1;
        //        List<CommiteeRolePermission> RolePermissions = commiteeRolePermissionService.GetAllPermission().Where(x => x.RoleId == RoleId).ToList();
        //        List<CommiteeUserPermissionDTO> commiteeUserPermissionDTO = new();
        //        foreach (var item in RolePermissions)
        //        {
        //            commiteeUserPermissionDTO.Add(new CommiteeUserPermissionDTO
        //            {
        //                RoleId = RoleId,
        //                PermissionId = item.PermissionId,
        //                UserId = int.Parse(commitee1.CurrenHeadUnitId.ToString()),
        //                Enabled = item.Permission.Enabled,
        //                CommiteeId = commitee1.CommiteeId,
        //                IsDelegated = false
        //            });
        //        }
        //        if (commiteeUserPermissionDTO.Count() > 0) commiteeUserPermissionService.Insert(commiteeUserPermissionDTO);
        //    }
        //    return commitee;

        //    //return _commiteeService.CustomInsert(entities.FirstOrDefault());
        //}



        [HttpGet("GetCommitteeDetailsWithValidityPeriod")]
        public CommiteeDTO GetCommitteeDetailsWithValidityPeriod(string id)
        {
            //int Id = 0;
            //int UserRoleId = 0;
            //string IdAndUserRoleId= Encription.DecryptStringAES(id);


            //int indexOf = IdAndUserRoleId.IndexOf("_");
            //int.TryParse(IdAndUserRoleId.Substring(0, indexOf),out Id);

            ////int length = transactionActionId.Length - indexOftransactionAction  + 1;
            //int.TryParse(IdAndUserRoleId.Substring(indexOf + 1),out UserRoleId);


            // transactionActionIdAfterCalling = int.Parse(transactionActionIdsplit);


            //if (_SessionServices.UserRoleIdOrignal != UserRoleId)
            //{

            //    throw new UnauthorizedAccessException();
            //}
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(id,false);
            return _commiteeService.GetCommitteeDetailsWithValidityPeriod(UserIdAndUserRoleId.Id, _session);

        }

        [HttpGet("GetCommitteeNamesById")]
        public ActionResult<CommiteeDTO> GetCommitteeNamesById(int committeeId)
        {
            return Ok(_commiteeService.GetCommitteeNames(committeeId));
        }

        [HttpGet("GeMasarUrl")]
        public string GeMasarUrl()
        {
            return _AppSettings.MasarURL;
        }
        [HttpGet("GetCommitteeLookup")]
        public DataSourceResult<LookUpDTO> GetCommitteeLookup([FromQuery] DataSourceRequest dataSourceRequest, string ParentId)
        {

            //int Id = 0;
            //int UserRoleId = 0;
            //string IdAndUserRoleId = Encription.DecryptStringAES(ParentId);


            //int indexOf = IdAndUserRoleId.IndexOf("_");
            //int.TryParse(IdAndUserRoleId.Substring(0, indexOf), out Id);

            ////int length = transactionActionId.Length - indexOftransactionAction  + 1;
            //int.TryParse(IdAndUserRoleId.Substring(indexOf + 1), out UserRoleId);

            //// transactionActionIdAfterCalling = int.Parse(transactionActionIdsplit);
            //if (_SessionServices.UserRoleIdOrignal != UserRoleId)
            //{

            //    throw new UnauthorizedAccessException();
            //}

            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(ParentId, true);
              
                if ((string.IsNullOrEmpty(ParentId)))
                {

                    DataSourceResult<LookUpDTO> committes = _commiteeService.GetCommitteeLookup(dataSourceRequest, true, null);
                    return committes;
                }
            
            
                else
                {
                    DataSourceResult<LookUpDTO> committes = _commiteeService.GetCommitteeLookup(dataSourceRequest, true, UserIdAndUserRoleId.Id);
                    return committes;
                }
            


        }
        [HttpGet("GetOrgnaztionLookup")]
        public DataSourceResult<LookUpDTO> GetOrgnaztionLookup([FromQuery] DataSourceRequest dataSourceRequest)
        {
            DataSourceResult<LookUpDTO> Orgnaztions = _commiteeService.GetOrgnaztionLookup(dataSourceRequest, true);
            return Orgnaztions;
        }
        [HttpGet("GetOrgnaztionFromSession")]
        public OrganizationSessionDTO GetOrgnaztionFromSession()
        {
            OrganizationSessionDTO Orgnaztion = _commiteeService.GetOrgnaztionFromSession();
            return Orgnaztion;
        }
        [HttpGet("GetCommitteeHeadUnitLookup")]
        public DataSourceResult<LookUpDTO> GetCommitteeHeadUnitLookup([FromQuery] DataSourceRequest dataSourceRequest , int? orgId)
        {
            DataSourceResult<LookUpDTO> committes = _commiteeService.GetCommitteeHeadUnitLookup(dataSourceRequest,orgId, true);
            return committes;
        }

        [HttpPost("GetMeetingHeadUnitLookupUserAndOrganization")]
        public List<LookUpDTO> GetMeetingHeadUnitLookupUserAndOrganization( List<int> orgsId)
        {
            List<LookUpDTO> committes = _commiteeService.GetMeetingHeadUnitLookupUserAndOrganization(orgsId, true);
            return committes;
        }

        [HttpGet("GetCommitteeHeadUnitLookupUserAndOrganization")]
        public DataSourceResult<LookUpDTO> GetCommitteeHeadUnitLookupUserAndOrganization([FromQuery] DataSourceRequest dataSourceRequest, int? orgId)
        {
            DataSourceResult<LookUpDTO> committes = _commiteeService.GetCommitteeHeadUnitLookupUserAndOrganization(dataSourceRequest, orgId, true);
            return committes;
        }

        [HttpGet("GetOrganizationLookup")]
        public DataSourceResult<LookUpDTO> GetOrganizationLookup([FromQuery] DataSourceRequest dataSourceRequest,bool fromAttendee)
        {
            DataSourceResult<LookUpDTO> committes = _commiteeService.GetOrganizationLookup(dataSourceRequest, fromAttendee , true);
            return committes;
        }

        [HttpGet("GetCommitteeRoles")]
        public List<CommiteeUsersRoleDTO> GetCommitteeRoles(string CommitteId,int userId)
        {
           UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId =_SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteId, true);
            return _commiteeService.GetCommitteeRolesNew(UserIdAndUserRoleId.Id, userId);
           
        }
        [HttpGet("GetCommitteWall")]
        public CommitteWallDTO GetCommitteWall(int take, int skip, DateTime? dateFrom, DateTime? dateTo, string CommitteId, string SearchText, bool asc = false)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteId, true);

            return _commiteeService.GetCommitteWall(take, skip, UserIdAndUserRoleId.Id, dateFrom, dateTo, SearchText, asc);
        }
        [HttpPost("Archive")]
        public bool Archive(string CommitteId)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteId, true);

            return _commiteeService.Archive(UserIdAndUserRoleId.Id);
        }
        [HttpGet("CommitteStatistic")]
        public List<CountResultDTO> CommitteStatistic(string CommitteId)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteId, false);
            
            return _commiteeService.CommitteStatistic(UserIdAndUserRoleId.Id);
        }
        [HttpPost("Disactive")]
        public bool Disactive(string CommitteId)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteId, true);

            return _commiteeService.Disactive(UserIdAndUserRoleId.Id);
        }
        [HttpPost("Extend")]
        public bool Extend(string CommitteId, DateTime DateTo)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteId, true);

            return _commiteeService.Extend(UserIdAndUserRoleId.Id, DateTo);
        }
        [HttpGet("GetActiviteyPerMonth")]
        public List<LineChartDTO> GetActiviteyPerMonthAsync(string CommitteId)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteId,true);

            return _commiteeService.GetActiviteyPerMonth(UserIdAndUserRoleId.Id);
        }
        [HttpGet("GetTasksPerUser")]
        public async Task<List<UserTaskCountDTO>> GetTasksPerUser(string CommitteId)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteId, true);

            return await _commiteeService.GetTasksPerUserAsync(UserIdAndUserRoleId.Id);
        }
        [HttpGet("GetAttachemntPerType")]
        public async Task<List<AttachemntTypeCountDTO>> GetAttachemntPerType(string CommitteId)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteId, true);

            return await _commiteeService.GetAttachemntPerType(UserIdAndUserRoleId.Id);
        }
        [HttpGet("GetCommittesTree")]
        public List<CommiteeDTO> GetCommittesTree()
        {
            return _commiteeService.GetCommittesTree();
        }
        [HttpGet("GetMeetingsByCommitteId")]
        public List<MeetingSummaryDTO> GetMeetingsByCommitteId(string committeId)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(committeId, true);

            return _commiteeService.GetMeetingsByCommitteId(UserIdAndUserRoleId.Id);
        }
    }
}
