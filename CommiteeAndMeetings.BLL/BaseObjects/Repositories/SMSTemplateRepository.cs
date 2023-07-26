using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.DAL.Domains;
using IHelperServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.BLL.BaseObjects.Repositories
{
    public class SMSTemplateRepository : BaseRepository<Smstemplate>, ISMSTemplateRepository
    {
        private MasarContext _context { get; set; }
        ISessionServices _sessionServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public SMSTemplateRepository(MasarContext mainDbContext, ISessionServices sessionServices, IHttpContextAccessor httpContextAccessor) : base(mainDbContext, sessionServices, httpContextAccessor)
        {
            _context = mainDbContext;
            _httpContextAccessor = httpContextAccessor;
            //   TransactionActionRecipientRepository = new TransactionActionRecipientRepository(mainDbContext, sessionServices);
        }

        public Smstemplate getTemplateByCode(SMSTemplateCodes Code)
        {
            return _context.Smstemplates.Where(w => w.Smscode == Code.ToString()).FirstOrDefault();
        }

        public SMS_Text_Params_DTO GetTemplateParams_Body(SMS_Text_Params_DTO SMSTemplate, SMSDelegationFieldsDTO DelegationFields)
        {
            SMS_Text_Params_DTO SMS_Text = new SMS_Text_Params_DTO();
            try
            {


                if (!String.IsNullOrEmpty(SMSTemplate.ParamtersString))
                {
                    // Replace  numbers as SMSDelegationFieldsDTO Fields as Ordered
                    // say 1 ,2 ,3 and   transactionNumberFormatted ,  From  , Date 
                    // Then replace 1 With transactionNumberFormatted , 2 With From , 3 With Date 
                    // Then return As Array Of String
                    // First Get String[] Parameters 
                    char[] seperator = new char[] { ',', '-', '_', '/' };
                    string[] numbers = SMSTemplate.ParamtersString.Split(seperator);
                    // Second Get Fields Values
                    Type tModelType = DelegationFields.GetType();
                    PropertyInfo[] Fields = tModelType.GetProperties();


                    // Third ForLoop For Fields 
                    for (int i = 1; i < Fields.Length + 1; i++)
                    {
                        string _i = i.ToString();

                        // And ThenPush Filed To SMS_Text.parameters if It Exisit
                        if (SMSTemplate.ParamtersString.IndexOf(_i) > -1)
                        {
                            string Fields_i = Fields[i - 1].GetValue(DelegationFields)?.ToString();
                            SMS_Text.Paramters.Add(Fields_i);
                            if (!String.IsNullOrEmpty(SMSTemplate.TextMessage))
                            {
                                SMSTemplate.TextMessage = SMSTemplate.TextMessage.Replace($"@{_i}", Fields_i);
                            }
                        }

                    }
                }

                if (!String.IsNullOrEmpty(SMSTemplate.TextMessage))
                {
                    SMS_Text.TextMessage = SMSTemplate.TextMessage;
                }

                return SMS_Text;
            }
            catch (Exception ex)
            {
                return new SMS_Text_Params_DTO { TextMessage = "Error" };
            }
        }
    }
}
