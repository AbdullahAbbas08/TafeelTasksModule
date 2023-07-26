namespace Models.Enums
{
    public enum WithDrawTransactionEnum
    {
        /// <summary>
        /// all الكل
        /// </summary>
        All = 1,
        /// <summary>
        /// منفذة مقبولة
        /// </summary>
        Accepted = 2,
        /// <summary>
        /// منفذة مرفوضة
        /// </summary>
        Rejected = 3,
        /// <summary>
        /// جديد لم يتم التعامل معه // غير منفذة
        /// </summary>
        NewRequest = 4,
        /// <summary>
        /// الطلبات المقدمة
        /// </summary>
        Requested = 5,
        /// <summary>
        /// جديد لم يتم التعامل معه على مستوى النظام بأكمله(هذا الخيار يظهر بصلاحية)  // غير منفذة
        /// </summary>
        NewRequestAllSysten = 6,

    }
}
