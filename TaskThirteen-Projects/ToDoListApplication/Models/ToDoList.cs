namespace ToDoListApplication.Models
{
    public class ToDoList
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Discription { get; set; }
        public DateTime DeadLine { get; set; }
        public string? File { get; set; }
    }
}
