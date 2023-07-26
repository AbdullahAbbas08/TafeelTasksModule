using System;
using System.ComponentModel.DataAnnotations;

namespace Models.ProjectionModels
{
    public class OldTransactionDTO
    {
        public int OldTransactionId { get; set; }
        public int TransactionId { get; set; }

        [MaxLength(25)]
        public string TransactionNumber { get; set; }

        public DateTimeOffset? TransactionDate { get; set; }
        public string Status { get; set; }
        public string Year { get; set; }
        public string Remarks { get; set; }
        public string TransactionBasis { get; set; }
        public string Subject { get; set; }
        public string Periority { get; set; }
        public string IncommingNumber { get; set; }
        public string ReceievedFrom { get; set; }
        public string AssignedTo { get; set; }
        public int? EntryID { get; set; }
        public string OutgoingNumber { get; set; }
        public string DeliveryType { get; set; }
        public string FinishFlag { get; set; }
        public string FileNo { get; set; }
        public string KeepingDate { get; set; }
        public string TransactionType { get; set; }
        public string TransacionHijriDate { get; set; }
        public string ConfidentialCode { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string OrgDocmntNo { get; set; }
        public string OrgDocmntDate { get; set; }
        public string SupplierDepName { get; set; }
        public string TransactionLocationDepName { get; set; }
    }
}
