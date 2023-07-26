using System;

namespace Models.ProjectionModels
{
    public class TransactionActionAttachmentDTO
    {
        public int TransactionActionAttachmentId { get; set; }
        public int TransactionActionId { get; set; }
        public int TransactionAttachmentId { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
    }
}
