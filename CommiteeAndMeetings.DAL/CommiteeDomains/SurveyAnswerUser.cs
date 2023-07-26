

using CommiteeAndMeetings.DAL.Domains;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("SurveyAnswerUsers", Schema = "Committe")]
    public class SurveyAnswerUser
    {
        [Key]
        public int SurveyAnswerUserId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int SurveyAnswerId { get; set; }
        public virtual SurveyAnswer SurveyAnswer { get; set; }
    }
}