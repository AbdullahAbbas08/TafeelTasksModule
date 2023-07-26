using System.ComponentModel;

namespace Models.Enums
{
    public enum QrFieldsEnum
    {
        // @TransactionType @TransactionTypeValue
        //@TransactionNumber @TransactionNumberValue
        //@TransactionDate @TransactionDateValue
        //@PhysicalAttachments @PhysicalAttachmentsCount
        [Description("@TransactionType")]
        TransactionType = 1,
        [Description("@TransactionNumber")]
        TransactionNumber = 2,
        [Description("@TransactionDate")]
        TransactionDate = 3,
        [Description("@PhysicalAttachments")]
        PhysicalAttachments = 4,

        [Description("@TransactionTypeValue")]
        TransactionTypeValue = 5,
        [Description("@TransactionNumberValue")]
        TransactionNumberValue = 6,
        [Description("@TransactionDateValue")]
        TransactionDateValue = 7,
        [Description("@PhysicalAttachmentsCount")]
        PhysicalAttachmentsCount = 8,



    }
}
