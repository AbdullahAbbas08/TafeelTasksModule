using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class DelegateForAllDTO
    {
        public int TransactionId { get; set; }
        public int TransactionActionId { get; set; }
        public bool IsShowAllTime { get; set; }
        public bool IsForAllEmployees { get; set; }
        public bool IsForAllOrganizations { get; set; }
        public bool IsForAllGroups { get; set; }

        /// <summary>
        //NULL For No Filter
        /// </summary>
        public bool? IsForMalesOnly { get; set; }

        public List<int> ExceptEmployeeIds { get; set; }
        public List<int> ExceptOrganizationIds { get; set; }
        public List<int> ExceptCommonGroupIds { get; set; }
        public bool IsNoteHidden { get; set; } = false;
        public List<TransactionActionAttachmentAnnotationDTO> TransactionActionAttachmentAnnotations { get; set; }

    }
}