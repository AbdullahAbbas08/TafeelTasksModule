namespace Models.ProjectionModels
{
    public class AssignmentCommentDTO
    {
        public int AssignmentCommentId { get; set; }
        public string Comment { get; set; }
        public int AssignmentId { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }
}
