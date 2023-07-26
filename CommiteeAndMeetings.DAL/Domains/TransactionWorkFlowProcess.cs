using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("TransactionWorkFlowProcess")]
    [Index(nameof(CreatedBy), Name = "IX_TransactionWorkFlowProcess_CreatedBy")]
    [Index(nameof(TransactionActionId), Name = "IX_TransactionWorkFlowProcess_TransactionActionId")]
    [Index(nameof(TransactionActionRecepientId), Name = "IX_TransactionWorkFlowProcess_TransactionActionRecepientId")]
    [Index(nameof(UpdatedBy), Name = "IX_TransactionWorkFlowProcess_UpdatedBy")]
    [Index(nameof(WorkFlowProcessId), Name = "IX_TransactionWorkFlowProcess_WorkFlowProcessId")]
    public partial class TransactionWorkFlowProcess
    {
        [Key]
        public int TransactionWorkFlowProcessId { get; set; }
        public int TransactionActionId { get; set; }
        public int TransactionActionRecepientId { get; set; }
        public int WorkFlowProcessId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.TransactionWorkFlowProcessCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(TransactionActionId))]
        [InverseProperty("TransactionWorkFlowProcesses")]
        public virtual TransactionAction TransactionAction { get; set; }
        [ForeignKey(nameof(TransactionActionRecepientId))]
        [InverseProperty(nameof(TransactionActionRecipient.TransactionWorkFlowProcesses))]
        public virtual TransactionActionRecipient TransactionActionRecepient { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.TransactionWorkFlowProcessUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [ForeignKey(nameof(WorkFlowProcessId))]
        [InverseProperty("TransactionWorkFlowProcesses")]
        public virtual WorkFlowProcess WorkFlowProcess { get; set; }
    }
}
