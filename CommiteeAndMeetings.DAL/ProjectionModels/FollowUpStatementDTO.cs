using System;

namespace Models.ProjectionModels
{
    public class FollowUpStatementDTO
    {
        public int FollowUpStatementId { get; set; }
        public int FollowUpId { get; set; }
        public int FollowUpStatementTypeId { get; set; }
        public int UserId { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public bool IsIndividual { get; set; }
        public int? FollowUpMessagingTypeId { get; set; }
        public string CreatedByNameAr { get; set; }
        public string CreatedByNameEN { get; set; }
        public string CreatedByNameFN { get; set; }

        public string MessageTypeAr { get; set; }
        public string MessageTypeEn { get; set; }
        public string MessageTypeFn { get; set; }

        public bool IsEmail { get; set; }
        public bool IsSMS { get; set; }
        public bool IsNotification { get; set; }


    }
}
