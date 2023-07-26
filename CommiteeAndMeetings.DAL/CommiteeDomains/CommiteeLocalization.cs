using CommiteeAndMeetings.DAL.ModelInterfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("CommiteeLocalizations", Schema = "Committe")]
    public class CommiteeLocalization : _BaseEntity, IAuditableInsert, IAuditableUpdate
    {
        [Key]
        public int CommiteeLocalizationId { get; set; }
        //[MaxLength(400)]
        //public string Category { get; set; }
        [MaxLength(100)]
        [Required]
        public string Key { get; set; }
        [Required]
        public string CommiteeLocalizationAr { get; set; }
        [Required]
        public string CommiteeLocalizationEn { get; set; }

        #region IAuditableUpdate

        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }


        #endregion IAuditableUpdate

        #region IAuditableInseret 
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

        #endregion
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }
    }
}
