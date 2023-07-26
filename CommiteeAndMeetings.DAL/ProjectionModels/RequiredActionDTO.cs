using Models.ProjectionModels;
using System.Collections.Generic;

namespace Models
{
    public class RequiredActionDTO
    {
        public int RequiredActionId { get; set; }
        public string RequiredActionNameAr { get; set; }
        public string RequiredActionNameEn { get; set; }

        public bool AllowedInCC { get; set; }
        public bool AllowedInTo { get; set; }

        public virtual ICollection<ActionRequiredActiondDTO> ActionRequiredActions { get; set; }
    }
}
