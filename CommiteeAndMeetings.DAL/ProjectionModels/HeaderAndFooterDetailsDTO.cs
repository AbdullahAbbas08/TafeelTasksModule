using Models.Enums;
using System;

namespace Models.ProjectionModels
{
    public class HeaderAndFooterDetailsDTO
    {
        public int HeaderAndFooterId { get; set; }
        public int TypeId { get; set; }
        public string ReportName { get; set; }
        public string ReportNameAr { get; set; }
        public string ReportNameEn { get; set; }
        public string ReportNameFn { get; set; }

        public string HTML { get; set; }

        public string Type
        {
            get
            {
                var enumValues = Enum.GetValues(typeof(HeaderAndFooterEnum));
                foreach (var item in enumValues) { if (TypeId == (int)item) return item.ToString(); }
                return "";
            }
        }
    }
}
