using CommiteeAndMeetings.DAL.ProjectionModels;
using DbContexts.MasarContext.ProjectionModels;
using Microsoft.AspNetCore.Http;

namespace IHelperServices
{
    public interface ISessionServices : _IHelperService
    {
        HttpContext HttpContext { get; set; }
        int? UserId { get; }
        string UserName { get; }
        int? RoleId { get; }
        int UserRoleId { get; set; }
        int UserRoleIdOrignal { get; set; }
        int? OrganizationId { get; }
        string UserTokenId { get; set; }
        //string MachineName { get; }
        //string MachineIP { get; }
        //string Browser { get; }
        //string Url { get; }
        string Culture { get; set; }
        bool CultureIsArabic { get; }

        string ApplicationType { get; set; }
        string EmployeeFullNameAr { get; }
        string EmployeeFullNameEn { get; }
        string OrganizationNameAr { get; }
        string OrganizationNameEn { get; }
        string RoleNameAr { get; }
        string RoleNameEn { get; }
        string ClientIP { get; }
        string FaxUserId { get; }

        void SetAuthTicket(string username, EncriptedAuthTicketDTO authTicket);
        EncriptedAuthTicketDTO GetAuthTicket(string username);
        UserIdAndRoleIdAfterDecryptDTO UserIdAndRoleIdAfterDecrypt(string encryptedValue, bool flag);
        void ClearSessionsExcept(params string[] keys);
    }
}
