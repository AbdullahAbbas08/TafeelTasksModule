namespace Models.ProjectionModels
{
    public class SSOLoginDTO
    {
        public string externalLoginUrl { get; set; }
        public string resultMessage { get; set; }

        public EncryotAndDecryptLoginUserDTO loginUser { get; set; }
    }
}
