using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class UsersDashboardStatistic
    {
        [Key]
        public int UserRoleId { get; set; }
        public long AllTasks { get; set; }
        public long CreatedTransactions { get; set; }
        public long DecisionAndCirculars { get; set; }
        public long Delayed { get; set; }
        public long Finished { get; set; }
        public long Received { get; set; }
        public long ReceivingExportingDelegations { get; set; }
        public long UnRecieved { get; set; }
        public long WithdrawalRequests { get; set; }
        [Column("TransactionsCC")]
        public long TransactionsCc { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }
    }
}
