namespace ToDoListApplication.ViewModels
{
    public class IndexVM
    {
        public List<ToDoList> Tasks { get; set; } = new();
        public Models.User User { get; set; }
    }
}
