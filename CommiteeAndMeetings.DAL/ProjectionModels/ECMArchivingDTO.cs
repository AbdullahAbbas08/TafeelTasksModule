using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class ECMArchivingDTO
    {
        public int ECMArchiveId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserId { get; set; }
        public string FolderEntryID { get; set; }
        public List<ECMArchivingPermissionDTO> Permissions { get; set; }
    }
}
