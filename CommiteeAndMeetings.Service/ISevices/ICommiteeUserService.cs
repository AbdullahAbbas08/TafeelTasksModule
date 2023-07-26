using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.Enums;
using CommiteeAndMeetings.Services.ISevices;
using LinqHelper;
using Models;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommiteeUserService : IBusinessService<CommiteeMember, CommiteeMemberDTO>
    {
        //List<LookUpDTO> GetExternalUsers(int take, int skip, string search);
        List<VwLookUpReturnUser> GetExternalUsers(int take, int skip, string search);
        //List<LookUpDTO> GetInternalUsers(int take, int skip, string search);
        List<VwLookUpReturnUser> GetInternalUsers(int take, int skip, string search);

        bool ConfirmChangeMemberState(string commiteeMemberId, MemberState memberState);
        //string ActiveDisactiveMember(string ids, bool active);
        string ActiveDisactiveMember(string ids, bool Active, MemberState memberState);
        DataSourceResult<CommiteeMember> GetActiveUsersByCommitteeId(DataSourceRequest dataSourceRequest, int committeeId);
        List<LookUpDTO> GetRoles(int take, int skip);
        DataSourceResult<CommiteeMemberDTO> GetAllByType(DataSourceRequest dataSourceRequest, bool external);
        CommiteeRoleDTO GetCommitteRole(int commiteeRoleId);
        UserDetailsDTO GetCommitteUser(int userId);
        DataSourceResult<CommiteeMemberDTO> GetAllWithCounts(DataSourceRequest dataSourceRequest, string SearchName);
        CommiteeUsersRoleDTO DelegateUser(int userId, int committeId, int committeMemberId, string Note, DateTimeOffset? ToDate);
        DataSourceResult<CommiteeMemberDTO> GetAllWithRoles(DataSourceRequest dataSourceRequest);
        UserProfileDetailsDTO GetUserProfile();
        UserDetailsDTO AddUserImage(int userId, byte[] ProfileImage, string ProfileImageMimeType);
        Task<User> GetCurrentUserAsync();
        Task<(bool Succeeded, string Error)> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        bool DisableDelegateUser(int userRoleID);
        string GetRoleName(int roleId, string v);
        bool CheckIfUserExixt(int commiteeId, int userId, int roleId);
        string GetUserRole(int userId, int committeeId);
        List<LookUpDTO> GetDeleatedUsers(int committeeId);
    }
}
