using System;

namespace Models.ProjectionModels
{
    public class ECMArchivingPermissionDTO
    {
        public int ECMArchivingPermitionId { get; set; }
        public int PermissionId { get; set; }
        public string PermissionCode { get; set; }
        public string PermissionNameAr { get; set; }
        public string PermissionNameEn { get; set; }
        public string PermissionNameFn { get; set; }

        public int ECMArchivingId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
