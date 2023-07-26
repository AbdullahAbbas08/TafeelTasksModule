namespace IHelperServices
{
    public interface ISmsServices : _IHelperService
    {
        //void Send(string to, string[] bodyParameters, string TextMessage, string templateCode);
        void SendSMS(string to, string[] bodyParameters, string TextMessage, string templateCode);
    }
}