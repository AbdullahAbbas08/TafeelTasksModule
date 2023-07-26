namespace Models.ProjectionModels
{
    public class OrganizationHasAccessDTO
    {

        public bool Status { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string TransactionAttachmentsEntryIDs { get; set; }
        public string TransactionAttachmentsNames { get; set; }

    }
}
