using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(DefaultCcrequiredActionId), Name = "IX_Actions_DefaultCCRequiredActionId")]
    [Index(nameof(DefaultToRequiredActionId), Name = "IX_Actions_DefaultToRequiredActionId")]
    public partial class Action
    {
        public Action()
        {
            Delegations = new HashSet<Delegation>();
        }

        [Key]
        public int ActionId { get; set; }
        [StringLength(400)]
        public string ActionCode { get; set; }
        [StringLength(400)]
        public string ActionNameAr { get; set; }
        [StringLength(400)]
        public string ActionNameEn { get; set; }
        public bool HasRecipient { get; set; }
        public int DisplayOrder { get; set; }
        public int? DefaultToRequiredActionId { get; set; }
        [Column("DefaultCCRequiredActionId")]
        public int? DefaultCcrequiredActionId { get; set; }
        [Column("AllowCCEmployees")]
        public bool? AllowCcemployees { get; set; }
        [Column("AllowCCExternalOrganization")]
        public bool? AllowCcexternalOrganization { get; set; }
        [Column("AllowCCInternalOrganization")]
        public bool? AllowCcinternalOrganization { get; set; }
        public bool? AllowMulti { get; set; }
        public bool? AllowToEmployees { get; set; }
        public bool? AllowToExternalOrganization { get; set; }
        public bool? AllowToInternalOrganization { get; set; }
        [Column("AllowInCaseCCTransactionInPreparation")]
        public bool? AllowInCaseCctransactionInPreparation { get; set; }
        public string ActionNameFn { get; set; }

        [ForeignKey(nameof(DefaultCcrequiredActionId))]
        [InverseProperty(nameof(RequiredAction.ActionDefaultCcrequiredActions))]
        public virtual RequiredAction DefaultCcrequiredAction { get; set; }
        [ForeignKey(nameof(DefaultToRequiredActionId))]
        [InverseProperty(nameof(RequiredAction.ActionDefaultToRequiredActions))]
        public virtual RequiredAction DefaultToRequiredAction { get; set; }
        [InverseProperty(nameof(Delegation.Action))]
        public virtual ICollection<Delegation> Delegations { get; set; }
    }
}
