using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.ProjectionModels
{
    public class AssignmentDTO
    {

        public long TransactionId { get; set; }
        public int TransactionActionId { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset? ExecutionDate { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public string fromUserProfileImage { get; set; }
        public string DirectedFromName { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? DirectedToUserId { get; set; }
        public int IsLate { get; set; }
        public string AssignmentIslate { get; set; }
        [NotMapped]
        public string Notes { get; set; }

        public IEnumerable<TransactionActionRecipientsDTO> taskRecipientsAndTO;
        public List<AttachmentViewDTO> transactionAttachments { get; set; }

    }
}
