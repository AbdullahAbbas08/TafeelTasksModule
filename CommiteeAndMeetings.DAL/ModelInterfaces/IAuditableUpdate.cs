using System;

namespace CommiteeAndMeetings.DAL.ModelInterfaces
{
    public interface IAuditableUpdate
    {
        int? UpdatedBy { get; set; }
        DateTimeOffset? UpdatedOn { get; set; }
    }
}