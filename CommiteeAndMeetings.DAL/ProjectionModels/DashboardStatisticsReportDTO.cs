namespace Models.ProjectionModels
{
    public class DashboardStatisticsReportDTO
    {
        //Counts
        public long CreatedTransactions { get; set; } //
        public long DelayedTransactions { get; set; } //
        public long ReceivingExportingDelegations { get; set; }
        public long WithdrawalRequests { get; set; }
        public long AllTasks { get; set; }
        public long Finished { get; set; }
        public long UnRecieved { get; set; } //
        public long Received { get; set; } //
        public long Delayed { get; set; }//
        public long DecisionAndCirculars { get; set; }//
        public long TransactionsIncoming { get; set; }
        public long TransactionsDelay { get; set; }
        public long TransactionsCC { get; set; }
        public long TransactionsClosed { get; set; }

        public long InProgress { get; set; }
        public long Outbox { get; set; }
        public long Inbox { get; set; }
        public long IsConfidential { get; set; }
        public long DecisionAndCircularsSeen { get; set; }//

    }
}
