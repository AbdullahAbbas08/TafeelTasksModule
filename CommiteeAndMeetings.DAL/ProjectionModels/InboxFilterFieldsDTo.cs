using System;

namespace Models.ProjectionModels
{
    public class InboxFilterFieldsDTo
    {
        public int? FromIncomingOrganizationId { get; set; }
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }
        public bool? IsEmployeeFilter { get; set; }
        public int? FromId { get; set; }
        /// <summary>
        /// Search in Search In Specific User By UserId
        /// </summary>
        public int? SearchInSpecificUser_UserId { get; set; }
        // 1 for All 2 For FollowUp
        public int FilterCase { get; set; } = 1;
    }
}
