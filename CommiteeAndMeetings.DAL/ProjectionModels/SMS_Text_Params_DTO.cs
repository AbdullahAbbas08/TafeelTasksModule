using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class SMS_Text_Params_DTO
    {
        public SMS_Text_Params_DTO()
        {
            Paramters = new List<string>();
        }
        public string TextMessage { get; set; }
        public string TemplateCode { get; set; }
        public bool IsActive { get; set; }

        public List<string> Paramters { get; set; }
        public string ParamtersString { get; set; }
    }
}
