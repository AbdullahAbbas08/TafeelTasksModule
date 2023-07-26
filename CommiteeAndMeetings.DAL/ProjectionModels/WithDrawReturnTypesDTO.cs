namespace Models.ProjectionModels
{
    public struct WithDrawReturnTypesDTO
    {
        /// <summary>
        /// تصدير مباشر
        /// </summary>
        public bool ExternalDelegationExecute { get; set; }
        /// <summary>
        ///  Excuted  تم السحب بنجاح
        /// </summary>
        public bool accepted { get; set; }
        /// <summary>
        ///  مازال السحب  تحت الطلب
        /// </summary>
        public bool underRequest { get; set; }
        /// <summary>
        ///   المعاملة قيد الاعتماد
        /// </summary>
        public bool underConfirmation { get; set; }
        /// <summary>
        ///   request  تم طلب السحب
        /// </summary>
        public bool request { get; set; }
        /// <summary>
        ///    Handled Error خطأ مع التفصيل
        /// </summary>
        public string error { get; set; }
        /// <summary>
        /// It Is WithDrawed تم سحب المعاملة من قبل
        /// </summary>
        public bool withDrawed { get; set; }
        /// <summary>
        /// لا يمكن سحب معاملة محفوظة
        /// </summary>
        public bool SavedTransaction { get; set; }


    }
}
