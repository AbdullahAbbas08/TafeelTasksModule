namespace DbContexts.MasarContext.ProjectionModels
{
    public class SystemSettingDTO
    {
        public int SystemSettingId { get; set; }
        public string SystemSettingCode { get; set; }
        public string SystemSettingNameAr { get; set; }
        public string SystemSettingNameEn { get; set; }
        public string SystemSettingNameFn { get; set; }

        public string SystemSettingValue { get; set; }
        public int? SystemSettingCategoryId { get; set; }
        public bool IsClientSide { get; set; }
        public bool IsHidden { get; set; }


    }
}
