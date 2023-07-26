using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeDatabase.Models.Domains;
using LinqHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommiteeService : IBusinessService<Commitee, CommiteeDTO>
    {
        List<CommiteeDTO> CustomInsert(CommiteeDTO commiteeDTO);
        int? GetCommitteRole(int CommitteId);
        DataSourceResult<LookUpDTO> GetCommitteeLookup(DataSourceRequest dataSourceRequest, bool v, int? ParentId);
        //DataSourceResult<LookUpDTO> GetCommitteeHeadUnitLookup(DataSourceRequest dataSourceRequest, bool v);
        DataSourceResult<LookUpDTO> GetCommitteeHeadUnitLookup(DataSourceRequest dataSourceRequest, int? orgId, bool WithTracking = true);
        DataSourceResult<LookUpDTO> GetOrganizationLookup(DataSourceRequest dataSourceRequest, bool fromAttendee, bool WithTracking = true);
        List<LookUpDTO> GetMeetingHeadUnitLookupUserAndOrganization(List<int> orgsList, bool WithTracking = true);
        DataSourceResult<LookUpDTO> GetCommitteeHeadUnitLookupUserAndOrganization(DataSourceRequest dataSourceRequest, int? orgId, bool WithTracking = true);
        //DataSourceResult<LookUpDTO> GetOrganizationLookup(DataSourceRequest dataSourceRequest, bool WithTracking = true);
        DataSourceResult<LookUpDTO> GetOrgnaztionLookup(DataSourceRequest dataSourceRequest, bool WithTracking);
        OrganizationSessionDTO GetOrgnaztionFromSession();
        CommiteeDTO GetCommitteeDetailsWithValidityPeriod(int id, ISession _session);
        CommiteeDTO GetCommitteeNames(int id);
        CommitteWallDTO GetCommitteWall(int take, int skip, int committeId, DateTime? dateFrom, DateTime? dateTo, string SearchText, bool asc);
        bool Archive(int committeId);
        List<CountResultDTO> CommitteStatistic(int committeId);
        bool Extend(int committeId, DateTime dateTo);
        Task<List<UserTaskCountDTO>> GetTasksPerUserAsync(int committeId);
        Task<List<AttachemntTypeCountDTO>> GetAttachemntPerType(int committeId);
        List<LineChartDTO> GetActiviteyPerMonth(int committeId);
        List<CommiteeUsersRoleDTO> GetCommitteeRoles(int committeId,int UserId);
        List<CommiteeUsersRoleDTO> GetCommitteeRolesNew(int committeId, int UserId);
        bool Disactive(int committeId);
        List<CommiteeDTO> GetCommittesTree();
        List<MeetingSummaryDTO> GetMeetingsByCommitteId(int committeId);
    }
   
}
