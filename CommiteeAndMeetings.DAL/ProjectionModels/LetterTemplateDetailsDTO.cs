using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class LetterTemplateDetailsDTO
    {
        public int LetterTemplateId { get; set; }
        public string LetterTemplateNameAr { get; set; }
        public string LetterTemplateNameEn { get; set; }
        public string LetterTemplateNameFn { get; set; }

        public string Description { get; set; }
        public string Text { get; set; }
        public bool? IsShared { get; set; }
        public bool? IsDefault { get; set; }

        public virtual ICollection<LetterTemplateOrganizationDTO> LetterTemplateOrganizations { get; set; }
    }
}
