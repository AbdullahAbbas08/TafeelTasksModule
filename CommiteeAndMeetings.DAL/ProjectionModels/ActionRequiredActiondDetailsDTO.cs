namespace Models
{
    public class ActionRequiredActiondDetailsDTO
    {
        public int ActionRequiredActionId { get; set; }
        public int ActionId { get; set; }
        public int RequiredActionId { get; set; }

        public virtual ActionDetailsDTO Action { get; set; }
        public virtual RequiredActionDTO RequiredAction { get; set; }
    }
}
