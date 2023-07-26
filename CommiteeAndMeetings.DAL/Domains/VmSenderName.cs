using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmSenderName
    {
        public int Id { get; set; }
        public int TransactionActionId { get; set; }
        public int? OrganizationId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public byte[] ProfileImage { get; set; }
        public int? UserId { get; set; }
    }
}
