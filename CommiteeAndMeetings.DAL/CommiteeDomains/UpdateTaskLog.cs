using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.CommiteeDomains
{
    [Table("UpdateTaskLog", Schema = "Committe")]

    public class UpdateTaskLog : _BaseEntity, IAuditableInsert, IAuditableUpdate, IAuditableDelete
    {
        [Key]
        public int UpdateTaskLogId { get; set; }
        public DateTimeOffset CancelDate { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string Notes { get; set; }

        public int CommiteeTaskId { get; set; }
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }
    }
}
