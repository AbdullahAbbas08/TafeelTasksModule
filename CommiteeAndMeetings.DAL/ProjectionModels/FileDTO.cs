namespace DbContexts.MasarContext.ProjectionModels
{
    public class FileDTO
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string OriginalName { get; set; }
        public string MimeType { get; set; }
        public int Size { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public virtual byte[] BinaryContent { get; set; }
    }
}
