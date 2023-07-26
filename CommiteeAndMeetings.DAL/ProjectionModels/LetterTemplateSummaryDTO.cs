namespace Models.ProjectionModels
{
    public class LetterTemplateSummaryDTO
    {
        public int LetterTemplateId { get; set; }
        public string LetterTemplateNameAr { get; set; }
        public string LetterTemplateNameEn { get; set; }
        public string LetterTemplateNameFn { get; set; }

        public string LetterTemplateName { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public bool? IsShared { get; set; }
        public bool? IsDefault { get; set; }
    }
}
