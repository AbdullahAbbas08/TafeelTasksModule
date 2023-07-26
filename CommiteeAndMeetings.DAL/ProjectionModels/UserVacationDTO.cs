using System;

namespace Models.ProjectionModels
{
    public class UserVacationDTO
    {
        public int UserVacationId { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public int StandByUserId { get; set; }
        public string StandByUserNameAr { get; set; }
        public string StandByUserNameEn { get; set; }
        public string StandByUserNameFn { get; set; }

        public string Description { get; set; }
    }
}
