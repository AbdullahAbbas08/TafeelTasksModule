using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class DeliveryCorrespondentDTO
    {
        public int DeliveryCorrespondentTransactionID { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int? AttachmentId { get; set; }
        public int? WhoIsEmpSign { get; set; }
        public string WhoIsExternalSign { get; set; }
        public string Signature { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Mobile { get; set; }
        public string SSN { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public AttachmentSummaryDTO Attachments { get; set; }
        public List<string> TransactionActionRecipientIds { get; set; }
    }
}
