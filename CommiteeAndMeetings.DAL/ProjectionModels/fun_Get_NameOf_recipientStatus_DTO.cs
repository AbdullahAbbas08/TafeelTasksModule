using System;

namespace Models.ProjectionModels
{
    public class fun_Get_NameOf_recipientStatus_DTO
    {
        public int Id { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int? RecipientStatusId { get; set; }
        public int TransactionActionRecipientStatusId { get; set; }
        public DateTimeOffset? TransactionActionRecipientStatusCreatedOn { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
    }
}
