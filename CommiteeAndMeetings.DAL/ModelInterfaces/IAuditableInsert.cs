using CommiteeDatabase.Models.Domains;
using System;

namespace CommiteeAndMeetings.DAL.ModelInterfaces
{
    public interface IAuditableInsert
    {
        int? CreatedBy { get; set; }
        int? CreatedByRoleId { get; set; }
        CommiteeUsersRole CreatedByRole { get; set; }
        DateTimeOffset? CreatedOn { get; set; }
    }
    public interface IAuditableInsertNoRole
    {
        int? CreatedBy { get; set; }
        DateTimeOffset? CreatedOn { get; set; }
    }
}