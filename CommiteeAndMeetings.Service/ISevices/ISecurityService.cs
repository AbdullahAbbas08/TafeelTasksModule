using CommiteeAndMeetings.Services.ISevices;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ISecurityService : IBusinessService
    {
        string GetSha256Hash(string input);
        string EncryptData(string textData);
        string DecryptData(string EncryptedText);
    }
}