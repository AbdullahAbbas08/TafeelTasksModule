using System;

namespace Models.ProjectionModels
{
    public class FollowUpDateModifiedDTO
    {
        public int followUpDateModifiedId { get; set; }
        public int followUpId { get; set; }
        public DateTimeOffset from { get; set; }
        public DateTimeOffset to { get; set; }
    }
}
