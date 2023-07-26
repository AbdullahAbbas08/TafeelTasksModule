using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmActionView
    {
        public int? Id { get; set; }
        public int ActionId { get; set; }
        [StringLength(400)]
        public string ActionCode { get; set; }
        [StringLength(400)]
        public string ActionNameAr { get; set; }
        [StringLength(400)]
        public string ActionNameEn { get; set; }
        public string ActionNameFn { get; set; }
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
        [Column("DefaultCCRequiredActionId")]
        public int? DefaultCcrequiredActionId { get; set; }
        public int? DefaultToRequiredActionId { get; set; }
        public int DisplayOrder { get; set; }
        public bool HasRecipient { get; set; }
        public int TransactionTypeId { get; set; }
        [Column("DefaultCCRequiredActionNameAr")]
        [StringLength(50)]
        public string DefaultCcrequiredActionNameAr { get; set; }
        [Column("DefaultCCRequiredActionNameEn")]
        [StringLength(50)]
        public string DefaultCcrequiredActionNameEn { get; set; }
        [Column("DefaultCCRequiredActionNameFn")]
        public string DefaultCcrequiredActionNameFn { get; set; }
        public string DefaultToRequiredActionNameFn { get; set; }
        [StringLength(50)]
        public string DefaultToRequiredActionNameAr { get; set; }
        [StringLength(50)]
        public string DefaultToRequiredActionNameEn { get; set; }
        [Column("AllowInCaseCCTransactionInPreparation")]
        public bool? AllowInCaseCctransactionInPreparation { get; set; }
    }
}
