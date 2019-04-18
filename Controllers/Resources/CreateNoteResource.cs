namespace notebook.Controllers.Resources
{
    public class CreateNoteResource
    {
        public string UserId { get; set; }
        public string CategoryId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}