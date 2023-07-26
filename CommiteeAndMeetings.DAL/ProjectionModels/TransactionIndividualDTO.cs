namespace DbContexts.MasarContext.ProjectionModels
{
    public class TransactionIndividualDTO
    {
        public int TransactionIndividualId { get; set; }
        public long TransactionId { get; set; }
        public int UserId { get; set; }
        public int? IndividualRelationshipId { get; set; }
        public string UserName { get; set; }
        public string UserProfilePricture { get; set; }
        public string IndividualRelationshipName { get; set; }
        public string Email { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameAr { get; set; }
        public string SSN { get; set; }
        public string IqamaNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string NationalityName { get; set; }
    }
}
