using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class TransactionActionAttachmentsWithRecipientsDTO
    {
        public int[] AttachmentsIds { get; set; }
        public int TransactionActionId { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public IEnumerable<TransactionActionAttachmentDTO> transactionActionAttachmentDTOs { get; set; }
        public IEnumerable<TransactionActionRecipientAttachmentDTO> transactionActionRecipientAttachmentDTOs { get; set; }
    }

}
