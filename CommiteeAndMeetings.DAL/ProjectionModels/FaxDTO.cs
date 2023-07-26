namespace Models.ProjectionModels
{
    public class FaxDTO
    {
        public string FaxId { get; set; }
        public string Subject { get; set; }
        public int PageCount { get; set; }
        public string From { get; set; }
        public bool IsTransfared { get; set; }
    }

}
