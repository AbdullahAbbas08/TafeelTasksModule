using CommiteeAndMeetings.DAL.Domains;
using Models.Enums;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces
{
    public interface ISMSTemplateRepository : IRepository<Smstemplate>
    {
        Smstemplate getTemplateByCode(SMSTemplateCodes EnumCode);
        SMS_Text_Params_DTO GetTemplateParams_Body(SMS_Text_Params_DTO sMSTemplate, SMSDelegationFieldsDTO Fields);
    }
}
