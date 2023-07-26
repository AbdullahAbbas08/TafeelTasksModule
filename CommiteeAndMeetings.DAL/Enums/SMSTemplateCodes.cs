using System.ComponentModel;

namespace Models.Enums
{
    public enum SMSTemplateCodes
    {


        [Description("001")]
        delegation = 1,
        [Description("002")]
        FactorAuth = 2,
        [Description("003")]
        SendTransactionNumber = 3,
        [Description("004")]
        followUpStatement = 4,
        [Description("006")]
        PasswordReset = 6,
        [Description("007")]
        OpenTransaction = 7,
        [Description("008")]
        AddTransactionIndividual = 8,
        [Description("009")]
        AcceptedTransactionIndividual = 9,
        [Description("010")]
        RejectedTransactionIndividual = 10,
        [Description("011")]
        FactorAuthMobile = 11


    }
}
