public class TodoItemDTO
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool IsCompleted { get; set; }

    public TodoItemDTO() { }
    public TodoItemDTO(Todo todoItem) =>
    (Id, Title, IsCompleted) = (todoItem.Id, todoItem.Title, todoItem.isCompleted);
}