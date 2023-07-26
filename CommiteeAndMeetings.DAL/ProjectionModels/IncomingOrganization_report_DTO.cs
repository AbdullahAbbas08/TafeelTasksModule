﻿using System;

namespace Models.ProjectionModels
{
    public class IncomingOrganization_report_DTO
    {
        public long Id { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public string Subject { get; set; }
        public int? IncomingOrganizationId { get; set; }
        public string IncomingLetterNumber { get; set; }
        public DateTimeOffset? IncomingLetterDate { get; set; }
        public DateTimeOffset TCreatedOn { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string OrganizationNameFn { get; set; }

        public int? StatusId { get; set; }
        public string StatusNameAr { get; set; }
        public string StatusNameEn { get; set; }
        public string StatusNameFn { get; set; }

        public int? DelegatedOrganizationId { get; set; }
        public string DelegatedOrganizationNameAr { get; set; }
        public string DelegatedOrganizationNameEn { get; set; }
        public string DelegatedOrganizationNameFn { get; set; }

        public long count { get; set; }
        public long TransactionId { get; set; }
        public int? RelatedTransactionsCount { get; set; }
        public string TransactionIdEncrypt { get { return EncryptHelper.Encrypt(this.TransactionId.ToString()); } }

    }
}
