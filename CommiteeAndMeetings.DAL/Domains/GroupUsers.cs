using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CommiteeAndMeetings.DAL.Domains
{
    public class GroupUsers
    {
        [Key]
        public int GroupUsersId { get; set; }
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }


        public virtual Group Group { get; set; }
        public virtual User User{ get; set; }
    }
}