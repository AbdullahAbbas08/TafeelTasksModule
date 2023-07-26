using CommiteeAndMeetings.DAL.Domains;
using CommiteeDatabase.Models.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.DAL.CommiteeDomains
{
    [Table("CommiteeTaskEscalation", Schema = "Committe")]
    public class CommiteeTaskEscalation : _BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CommiteeTaskEscalationIndex { get; set; }
        [ForeignKey("MainAssinedUser")]
        public int MainAssinedUserId { get; set; }
        public int DelayPeriod { get; set; }
        [ForeignKey("ComiteeTaskCategory")]
        public int? ComiteeTaskCategoryId { get; set; }
        public virtual ComiteeTaskCategory ComiteeTaskCategory { get; set; }
        [ForeignKey("NewMainAssinedUser")]
        public int NewMainAssinedUserId { get; set; }
        public virtual User MainAssinedUser { get; set; }
        public virtual User NewMainAssinedUser { get; set; }




    }
}
