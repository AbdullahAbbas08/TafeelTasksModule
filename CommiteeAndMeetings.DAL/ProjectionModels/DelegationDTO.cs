using CommiteeAndMeetings.DAL.Domains;
using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{


    public class DelegationDTO
    {

        public int TransactionActionId { get; set; }
        public string TransactionActionNumber { get; set; }
        public long TransactionId { get; set; }
        public int ActionId { get; set; }
        public int? ReferrerTransactionActionId { get; set; }
        public int? ReferrerTransactionActionRecipientId { get; set; }
        public string ActionNumber { get; set; }
        /// <summary>
        /// الجهة المنشئة لهذا السجل، سواء يوزر
        /// </summary>
        public int? DirectedFromUserId { get; set; }
        public string DirectedFromUserNameAr { get; set; }
        public string DirectedFromUserNameEn { get; set; }

        /// <summary>
        /// الجهة المنشئة لهذا السجل، أورجانايزيشن
        /// </summary>
        public int? DirectedFromOrganizationId { get; set; }
        public string DirectedFromOrganizationNameAr { get; set; }
        public string DirectedFromOrganizationNameEn { get; set; }

        /// <summary>
        /// ملاحظات
        /// </summary>
        //public string Notes { get; set; }

        /// <summary>
        /// الدور الوظيفي للمستخدم وقت إنشاء هذا السجل
        /// </summary>
        public int? CreatedByUserRoleId { get; set; }
        public string CreatedByUserRoleName { get; set; }

        /// <summary>
        /// يتم تفعيله والعمل به من تاريخ معين
        /// </summary>
        public DateTimeOffset? DisabledUntil { get; set; }

        /// <summary>
        /// رقم المعاملة وقت التصدير
        /// </summary>

        public string OutgoingTransactionNumber { get; set; }

        /// <summary>
        /// (نوع المعاملة وقت التصدير (وارد عام، قرار، تعميم،.. إلخ
        /// </summary>
        public int? OutgoingTransactionTypeId { get; set; }

        /// <summary>
        /// (درجة أهمية المعاملة وقت التصدير (عاجل وهام، هام جدا،.. إلخ
        /// </summary>
        public int? OutgoingImportanceLevelId { get; set; }

        /// <summary>
        /// معاملة سرية وقت التصدير
        /// </summary>
        public bool OutgoingIsConfidential { get; set; }
        public bool isEmployee { get; set; }
        public int TransactionTypeId { get; set; }
        public bool? IsTransaction { get; set; } = true;
        public bool acceptPreviousTRAR { get; set; } = false;
        public List<TransactionActionRecipientsDTO> transactionActionRecipientsDTO { get; set; }
        public List<TransactionActionAttachmentDTO> transactionActionAttachmentDTO { get; set; }
        public List<TransactionActionAttachmentAnnotationDTO> TransactionActionAttachmentAnnotations { get; set; }

        public List<AttachmentViewDTO> transactionActionAttachmentViewDTO { get; set; }
        // used In Backend For workFlow
        public List<TransactionActionRecipient> Recipients { get; set; }

        public bool? isFollowUp { get; set; }
        public int? followUpOrganizationId { get; set; }
        public DateTimeOffset? followUpfinishedDate { get; set; }
        public EmailDetailsUrlsDTO EmailDetailsUrlsDTO { get; set; } = new EmailDetailsUrlsDTO();

        public bool DelegationFromTransition { get; set; } = false;
        public bool reject_fromPreparation { get; set; }
        public bool ApplyAcceptConfirmation { get; set; }
        /// <summary>
        /// Use This Permission If You Have Permisssion To Skip
        /// Another Confirmation Organization Of WorkFlow
        /// </summary>
        public bool SkipConfiramtion { get; set; } = false;
        public bool IssCCWorkFlow { get; set; } = false;
        public bool IsAssignedWorkFlowProcess { get; set; } = false;
        public bool IsAssignedWorkFlowProcessFromPreparationOrg { get; set; } = false;

        public int AssignedWorkFlowProcessId { get; set; }
        public bool IgnoreWorkFlowProcess { get; set; } = false;
        public bool reject_fromConfirmation { get; set; } = false;
        //public bool IsNoteHidden { get; set; } = false;
        public string CommitteId { get; set; }

        /// <summary>
        /// IsSave Committee Minute -- حفظ محضر الاجتماع مع المعاملة
        /// </summary>
        public bool IsSaveCommitteeMinutes { get; set; }
    }
}
