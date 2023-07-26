using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Enums;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeDatabase.Models.Domains;
using LinqHelper;
using System;
using System.Collections.Generic;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommiteeTaskService : IBusinessService<CommiteeTask, CommiteeTaskDTO>
    {
        bool Complete(int ids, string reason);

        CommiteeTaskDTO GetDetailsById(int CommiteeTaskId);
        DataSourceResult<CommiteeTaskDTO> GetAllwithfilters(DataSourceRequest dataSourceRequest, TaskFilterEnum requiredTasks, ParamsSearchFilterDTO paramsSearchFilterDTO = null, int? userId = null,int? organizationId = null,bool WithTracking = true);
        List<CommiteeTaskDTO> GetAllForPrint(TaskFilterEnum requiredTasks, int? CommiteeId, int? ComiteeTaskCategoryId, string SearchText, int? userId = null, ParamsSearchFilterDTO paramsSearchFilterDTO = null);

        List<CommiteeTaskDTO> GetAllForCalender(int? CommiteeId, int? ComiteeTaskCategoryId);

        List<CountResultDTO> getComitteeTaskStatistics(int? OrganizationId,int? userId,int? committeeId, DateTime? ValiditayPeriodFrom, DateTime? ValidatiyPeriodTo);
        CommiteetaskMultiMissionDTO InsertMultiMissionToTask(int commiteeTaskId, CommiteetaskMultiMissionDTO entity);
        CommiteetaskMultiMissionDTO changeState(int missionId);
        byte[] Export(TaskFilterEnum requiredTasks, int? UserIdEncrpted , bool ExportWord = true);

    }
}
