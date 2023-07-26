
namespace Models.ProjectionModels
{
    public class EmployeAchievementReportDTO
    {
        public int Id { get; set; }
        public string fullName { get; set; }
        public string profileImage { get; set; }
        public int reciviedCount { get; set; }
        public int unReciviedCount { get; set; }
        public int savedCount { get; set; }
        public int sentCount { get; set; }
        public int draftCount { get; set; }
        public int isLate { get; set; }

    }
}
