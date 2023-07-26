using System.Collections.Generic;

namespace DbContexts.MasarContext.ProjectionModels
{
    public class Lookup
    {
        public object Id { get; set; }
        public string Text { get; set; }
        public bool IsDefault { get; set; }
        public bool IsCategory { get; set; }
        public bool? Enabled { get; set; }
        public string EmployeeNumber { get; set; }
        public string ReferenceNumberName { get; set; }
        public Lookup[] Children { get; set; }
        public LookupAdditional Additional { get; set; }
    }

    public class LookupAdditional
    {
        public string Description { get; set; }
        public int? ImageId { get; set; }
        public string ImageUrl { get; set; }
        public string MimeType { get; set; }
        public byte[] Image { get; set; }
        public List<Lookup> Ancestors { get; set; }
        public object Data { get; set; }
        public bool? AllowMulti { get; set; }
        public bool? AllowToEmployees { get; set; }
        public bool? AllowToExternalOrganization { get; set; }
        public bool? AllowToInternalOrganization { get; set; }
        public bool? AllowCCEmployees { get; set; }
        public bool? AllowCCExternalOrganization { get; set; }
        public bool? AllowCCInternalOrganization { get; set; }
        public bool? IsReferenceRequired { get; set; }

        public int? DefaultToRequiredActionId { get; set; }
        public int? DefaultCCRequiredActionId { get; set; }

        public string DefaultCCRequiredActionNameAr { get; set; }
        public string DefaultCCRequiredActionNameEn { get; set; }
        public string DefaultCCRequiredActionNameFn { get; set; }

        public string DefaultToRequiredActionNameAr { get; set; }
        public string DefaultToRequiredActionNameEn { get; set; }
        public string DefaultToRequiredActionNameFn { get; set; }

        //RequiredActions
        public bool? AllowedInTO { get; set; }
        public bool? AllowedInCC { get; set; }

        //user
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
