using Models.ProjectionModels;
using System.Collections.Generic;

namespace Models
{
    public class RequiredActionDetailsDTO
    {
        public int RequiredActionId { get; set; }
        public string RequiredActionNameAr { get; set; }
        public string RequiredActionNameEn { get; set; }
        public string RequiredActionNameFn { get; set; }

        public bool AllowedInCC { get; set; }
        public bool AllowedInTo { get; set; }
        public bool AllowedInVip { get; set; }
        public string Code { get; set; }
        public bool IsForPreparation { get; set; }

        public virtual ICollection<ActionRequiredActiondDTO> ActionRequiredActions { get; set; }
    }
}
