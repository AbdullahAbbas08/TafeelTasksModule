using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.Service.ISevices;
using DbContexts.AuditDbContext;
using IHelperServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class AuditService
    {
        protected readonly string _AuditBaseUrl;
        protected readonly AppSettings _appSettings;
        private readonly IHelperServices.ISessionServices _sessionServices;

        public HttpContext HttpContext
        {
            get => _contextAccessor.HttpContext;
            set => _contextAccessor.HttpContext = value;
        }
        private IHttpContextAccessor _contextAccessor;

        public AuditService(IOptions<AppSettings> appSettings, IHelperServices.ISessionServices sessionServices, IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            this._appSettings = appSettings.Value;
            this._sessionServices = sessionServices;
            this._AuditBaseUrl = _appSettings.AuditSettings.BaseUrl;
        }

        public void AddAudit(Models.AuditDTOs.AuditTrail Audit)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(this._AuditBaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsync("api/Audits/AddNewAudit", new StringContent(JsonConvert.SerializeObject(Audit), Encoding.UTF8, "application/json")).Result;
        }

        public void SaveAuditTrail(object previousStateObject, object currentStateObject, string entity, string action, string Number, long? transactionId)
        {
            try
            {

                //var newobj = JObject.FromObject(currentStateObject);
                CommitteAudit audit = new CommitteAudit()
                {

                    ActionName = action,
                    CreatedDate = DateTime.Now,
                    ForignKeys = null,
                    NewValue = null /*HelperFunction.SerializeObjectWithTransactionNumberFormatted(currentStateObject)*/,
                    OldValue = JsonConvert.SerializeObject(previousStateObject),
                    OrganizationName = _sessionServices.OrganizationNameAr,
                    RoleName = _sessionServices.RoleNameAr,
                    TableName = entity,
                    UserName = _sessionServices.EmployeeFullNameAr,
                    LoginName = _sessionServices.UserName,
                    IP = HttpContext.Connection.RemoteIpAddress.ToString(),
                    TransactionNumberFormatted = Number,
                    TransactionId = transactionId
                };
                using (var cont = new AuditContext())
                {
                    cont.Audits.Add(audit);
                    cont.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
