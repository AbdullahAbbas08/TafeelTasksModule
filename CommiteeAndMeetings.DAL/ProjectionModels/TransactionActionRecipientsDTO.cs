using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class TransactionActionRecipientsDTO
    {
        public int TransactionActionRecipientId { get; set; }
        public int TransactionActionId { get; set; }

        public int? DirectedToUserId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? RequiredActionId { get; set; }
        public string RequiredActionAr { get; set; }
        public string RequiredActionEn { get; set; }
        public string RequiredActionFn { get; set; }
        public bool IsCC { get; set; }
        public int? CorrespondentUserId { get; set; }
        public string CorrespondentUserNameAr { get; set; }
        public string CorrespondentUserNameEn { get; set; }
        public string CorrespondentUserNameFn { get; set; }

        public int? RecipientStatusId { get; set; }
        public int? RecipientStatusChangedBy { get; set; }
        public DateTimeOffset? RecipientStatusChangedOn { get; set; }
        public bool IsUrgent { get; set; }
        public int? UrgencyDaysCount { get; set; }
        public string Notes { get; set; }
        public string NotesEn { get; set; }
        public string NotesFn { get; set; }
        public bool SendNotification { get; set; }
        public int CreatedBy { get; set; }
        public string FullName { get; set; }
        public bool? IsOuterOrganization { get; set; }
        public string ProfileImage { get; set; }
        public bool IsHidden { get; set; } = false;
        public bool IsNoteHidden { get; set; } = false;
        public IEnumerable<TransactionActionRecipientAttachmentDTO> transactionActionRecipientAttachmentDTO { get; set; }
        public IEnumerable<AttachmentViewDTO> transactionActionAttachmentRecipientViewDTO { get; set; }

    }
}
