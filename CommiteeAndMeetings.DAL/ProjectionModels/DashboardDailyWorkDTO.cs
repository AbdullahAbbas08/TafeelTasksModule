namespace Models.ProjectionModels
{
    public class DashboardDailyWorkDTO
    {
        public long Inbox { get; set; }
        public long Outbox { get; set; }
        public long Saved { get; set; }
        public long Withdrawal { get; set; }
        public long Old { get; set; }
        public long FollowUp { get; set; }
    }
}
