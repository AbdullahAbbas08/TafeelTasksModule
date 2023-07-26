using System.ComponentModel;

namespace Models.Enums
{
    public enum AttachmentsWaterMarkEnum
    {
        [Description("UserID")]
        UserID = 1,
        [Description("OrganizationID")]
        OrganizationID = 2,
        [Description("RoleID")]
        RoleID = 3,
        [Description("ClientIP")]
        ClientIP = 4,
        [Description("DateTime")]
        DateTime = 5
    }
}
