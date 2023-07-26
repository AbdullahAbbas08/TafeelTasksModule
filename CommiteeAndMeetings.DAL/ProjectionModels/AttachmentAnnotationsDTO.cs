using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class AttachmentAnnotationsDTO
    {
        public IEnumerable<int> AttachmentIds { get; set; }
        public long? TransactionId { get; set; }
        public int? TransactionActionId { get; set; }
        public int? TransactionActionRecipientId { get; set; }
    }
}
