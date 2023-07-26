using System;

namespace Models.ProjectionModels
{
    /// <summary>
    /// 
    /// </summary>
    public class FilterGetDecisionDTO
    {

        public int @Page { get; set; }
        public int @PageSize { get; set; }

        public bool @IsEmployee { get; set; }

        public int? @TypeId { get; set; }
        public int? @ClassificationId { get; set; }
        public string @TransactionSubject { get; set; }

        public int? @FromOrgId { get; set; }
        public int? @FromUserId { get; set; }
        public string searchText { get; set; }


        public int? @Year { get; set; }
        public int? @Month { get; set; }
        public DateTimeOffset? @Date { get; set; }
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }

        public DecisionsStatusFilterEnum @DecisionsStatusFilterEnum { get; set; }


    }
}
