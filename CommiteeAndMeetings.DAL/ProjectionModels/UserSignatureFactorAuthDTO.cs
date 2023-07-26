using System;

namespace Models.ProjectionModels
{
    public class UserSignatureFactorAuthDTO
    {
        public int Id { get; set; }
        public string FactorAuthCode { get; set; }
        public DateTime FactorAuthDate { get; set; }
        public int UserId { get; set; }
    }
}
