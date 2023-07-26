using System;

namespace Models.ProjectionModels
{
    public class AllowedCountryDTO
    {
        public int AllowedCountryId { get; set; }
        public int CountryId { get; set; }
        public CountryDTO Country { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
    }
}
