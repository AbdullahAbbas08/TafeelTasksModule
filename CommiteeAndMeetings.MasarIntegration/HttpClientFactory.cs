using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.ProjectionModels;
using DbContexts.MasarContext.ProjectionModels;
using Models;
using Models.ProjectionModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace CommiteeAndMeetings.MasarIntegration
{
    public class HttpClientFactory
    {
        public AccessToken Login(string absoluteUrl)
        {
            UserLoginModel userLoginModel = new UserLoginModel
            {
                Username = System.Configuration.ConfigurationManager.AppSettings["userName"],
                Password = System.Configuration.ConfigurationManager.AppSettings["password"]
            };
            string json = JsonConvert.SerializeObject(userLoginModel, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            return JsonConvert.DeserializeObject<AccessToken>(APIsInterface.Post("api/Account/Login", new Dictionary<string, string> { { "culture", "ar" } }, data, null, absoluteUrl));
        }

        public AuthTicketDTO GetUserAuthTicket(bool personal, string accessToken, string absoluteUrl)
        {
            return JsonConvert.DeserializeObject<AuthTicketDTO>(APIsInterface.Get("api/Account/GetUserAuthTicket", new Dictionary<string, string>() { { "personal", personal.ToString() } }, accessToken, absoluteUrl));
        }

        public string GetMapValue(string tableName, string value, bool isInbound, string accessToken, string absoluteUrl)
        {
            return APIsInterface.Get("api/MasarLookUpMap/GetMapValues", new Dictionary<string, string>() { { "tableName", tableName }, { "value", value }, { "isInbound", isInbound.ToString() } }, accessToken, absoluteUrl);
        }

        public TransactionDetailsDTO SaveTransaction(TransactionDetailsDTO transaction, string accessToken, string absoluteUrl)
        {
            string json = JsonConvert.SerializeObject(transaction, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            var content = APIsInterface.Post("api/Transaction/SaveTransactions", null, data, accessToken, absoluteUrl);
            return new TransactionDetailsDTO();
        }

        public IEnumerable<AttachmentSummaryDTO> UploadDocument(MultipartFormDataContent multipartFormDataContent, Dictionary<string, string> parameters, string accessToken, string absoluteUrl)
        {
            return JsonConvert.DeserializeObject<IEnumerable<AttachmentSummaryDTO>>(APIsInterface.Post("api/Document/Upload", parameters, multipartFormDataContent, accessToken, absoluteUrl));
        }

        public List<TransactionAttachmentDTO> InsertAttachments(List<TransactionAttachmentDTO> transaction, string accessToken, string absoluteUrl)
        {
            string json = JsonConvert.SerializeObject(transaction, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            return JsonConvert.DeserializeObject<List<TransactionAttachmentDTO>>(APIsInterface.Post("api/TransactionAttachments/Insert", null, data, accessToken, absoluteUrl));
        }

        public bool InsertDelegation(DelegationDTO delegationDTO, string accessToken, string absoluteUrl)
        {
            string json = JsonConvert.SerializeObject(delegationDTO, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            return JsonConvert.DeserializeObject<bool>(APIsInterface.Post("api/Delegation/InsertDelegation", null, data, accessToken, absoluteUrl));
        }

        public bool DeactivateTransaction(long transactionId, bool isCanceled, string accessToken, string absoluteUrl)
        {
            return JsonConvert.DeserializeObject<bool>(APIsInterface.Post("api/Transaction/ActivateorDeactivateDrafts", new Dictionary<string, string> { { "transactionActionId", transactionId.ToString() }, { "isCancelled", isCanceled.ToString() } }, null, accessToken, absoluteUrl));
        }



    }
}
