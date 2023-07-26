namespace CommiteeAndMeetings.DAL.CommiteeDomains
{
    public class UpdateTaskLogAssistantUser : UpdateTaskLog
    {
        public int AssistantUserId { get; set; }
        public string Action { get; set; }
    }
}
