namespace Models.Enums
{
    public enum RecipientStatusesEnum
    {
        ///<summary>Viewed مشاهدة</summary>
        Viewed = 1,
        ///<summary>Received مستلمة</summary>
        Received = 2,
        ///<summary>Rejected مرفوضة </summary>
        Rejected = 3,
        ///<summary>Sent صدرت </summary>
        Sent = 4,
        ///<summary>Seen تم الاطلاع </summary>
        Seen = 5,
        ///<summary>Saved حفظ</summary>
        Saved = 6,
        ///<summary>Withdrawed سحبت</summary>
        Withdrawed = 7,
        ///<summary>Required for withdrawal مطلوبة للسحب</summary>
        RequiredWithdrawal = 8,
        ///<summary>Accepted مقبولة</summary>
        Accepted = 9,
        ///<summary>Finished Assignment</summary>
        FinishAssignment = 10,
        ///<summary>مطبوعة </summary>
        Printed = 11,
        ///<summary>Cancelled ملغي</summary>
        Cancelled = 12,
        ///<summary>Related Sent صدرت للارتباط</summary>
        RelatedSent = 13,
        ///<summary> Withdrawel from Export استرجاع من التصدير</summary>
        ExportWithdrawel = 14,
        ///<summary> Under Preparation قيد التجهيز</summary>
        UnderPreparation = 15,
        ///<summary> under Confirmation قيد الاعتماد</summary>
        underConfirmation = 16,
        ///<summary> Accept Confirmation  قبول الاعتماد</summary>
        AcceptConfirmation = 17,
        ///<summary> Reject Confirmation رفض الاعتماد </summary>
        RejectConfirmation = 18,
        ///<summary> Sent By Fax ارسلت بالفاكس </summary>
        SentByFax = 19,
        ///<summary> Sent By Yasser صدرت عن طريق يسر </summary>
        SentByYasser = 20,
    }
}
