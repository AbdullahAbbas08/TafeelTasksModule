using System;

namespace CommiteeAndMeetings.DAL.ModelInterfaces
{
    public interface IAuditableDelete
    {
        int? DeletedBy { get; set; }
        DateTimeOffset? DeletedOn { get; set; }
    }
}