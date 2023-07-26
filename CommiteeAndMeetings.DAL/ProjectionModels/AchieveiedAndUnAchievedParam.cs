using System;

namespace Models.ProjectionModels
{
    public class AchieveiedAndUnAchievedParam
    {
        public bool Achived { get; set; }
        public string org_ids { get; set; }
        public string user_ids { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Skip { get; set; }
        public int PageSize { get; set; }
    }
}
