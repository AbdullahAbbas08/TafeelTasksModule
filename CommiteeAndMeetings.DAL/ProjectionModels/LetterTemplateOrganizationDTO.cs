using System;

namespace Models.ProjectionModels
{
    public class LetterTemplateOrganizationDTO
    {


        public int LetterTemplateOrganizationId { get; set; }
        public int LetterTemplateId { get; set; }
        public int OrganizationId { get; set; }


        public virtual LetterTemplateSummaryDTO LetterTemplate { get; set; }

        public virtual DbContexts.MasarContext.ProjectionModels.OrganizationDetailsDTO Organization { get; set; }



        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }



        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

    }
}
