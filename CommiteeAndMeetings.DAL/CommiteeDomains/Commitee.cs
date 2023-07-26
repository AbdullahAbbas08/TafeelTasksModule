using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("Commitees", Schema = "Committe")]
    public class Commitee : _BaseEntity, IAuditableInsertNoRole, IAuditableUpdate, IAuditableDelete
    {
        public Commitee()
        {
            ValidityPeriod = new List<ValidityPeriod>();
            Members = new List<CommiteeMember>();
            Meetings = new List<Meeting>();
        }
        [Key]
        public int CommiteeId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? CommiteeTypeId { get; set; }
        public virtual CommiteeType CommiteeType { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int? ParentCommiteeId { get; set; }
        public virtual Commitee ParentCommitee { get; set; }

        // Orgnaztion on Masar
        public int? DepartmentLinkId { get; set; }
        public virtual Organization DepartmentLink { get; set; }

        public int? CurrentStatusId { get; set; }
        public virtual CurrentStatus CurrentStatus { get; set; }
        public DateTimeOffset? CurrentStatusDate { get; set; }
        public int? CurrentStatusReasonId { get; set; }
        public virtual CurrentStatusReason CurrentStatusReason { get; set; }
        public bool EnableTransactions { get; set; }
        public bool EnableDecisions { get; set; }
        public int? CurrenHeadUnitId { get; set; }
        [ForeignKey("CurrenHeadUnitId")]
        public virtual User CurrenHeadUnit { get; set; }

        public int? CommitteeSecretaryId { get; set; }
        [ForeignKey("CommitteeSecretaryId")]
        public virtual User CommitteeSecretary { get; set; }
        public bool IsSecrete { get; set; }
        //   public List<StatusHistory> StatusHistories { get; set; }
        public virtual List<ValidityPeriod> ValidityPeriod { get; set; }
        public virtual List<CommiteeMember> Members { get; set; }
        public virtual List<Survey> Surveys { get; set; }
        public virtual List<CommiteeSavedAttachment> Attachments { get; set; }
        public virtual List<CommiteeTask> Tasks { get; set; }
        //  public virtual List<Transaction> Transaction { get; set; }

        public virtual List<CommiteeUsersRole> CommiteeRoles { get; set; }
        public virtual List<Meeting> Meetings { get; set; }
        public virtual List<CommiteeUsersPermission> CommiteePermissions { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }

    }
}
