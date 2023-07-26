using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class AnnouncementDTO
    {
        public bool IsForEmployees { get; set; }
        public bool IsForOrganizations { get; set; }
        public bool IsForGroups { get; set; }
        public bool IsForAll { get; set; }
        public bool IsForUsersOnOrganizations { get; set; }
        public List<int> EmployeeIds { get; set; }
        public List<int> OrganizationIds { get; set; }
        public List<int> CommonGroupIds { get; set; }
        public long TransactionId { get; set; }
        public int TransactionActionId { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset? ExecutionDate { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public string fromUserProfileImage { get; set; }
        public string DirectedFromName { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? DirectedToUserId { get; set; }
        public IEnumerable<AttachmentViewDTO> transactionActionRecipientAttachment { get; set; }

        public IEnumerable<TransactionActionRecipientsDTO> announcementRecipientsAndTO;
    }
}
