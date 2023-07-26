using System;

namespace Models.ProjectionModels
{
    public class FollowUpMapDTO
    {
        public int FollowUpId { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public DateTimeOffset? followUpCreatedOn { get; set; }
        public DateTimeOffset? followUpFinishedOn { get; set; }
        public string OrganizationFollowUpOwnerName { get; set; }
    }
}
