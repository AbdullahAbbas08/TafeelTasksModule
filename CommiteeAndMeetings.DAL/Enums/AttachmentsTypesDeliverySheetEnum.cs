using System.ComponentModel;

namespace Models.Enums
{
    public enum AttachmentsTypesDeliverySheetEnum
    {
        [Description("PhysicalCount")]
        PhysicalCount = 1,
        [Description("LetterCount")]
        LetterCount = 2,
        [Description("DocumentCount")]
        DocumentCount = 3
    }
}
