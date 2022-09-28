namespace Assignment3.Entities;

public class Task
{
    public Task(string title, int? userId, string? description)
    {
        Title = title;
        UserId = userId;
        Description = description;
        Created = DateTime.UtcNow;
        StateUpdated = DateTime.UtcNow;
        State = State.New;
    }

    public Task(string title, int? userId, string? description, ICollection<Tag>? tags)
    {
        Title = title;
        UserId = userId;
        Description = description;
        Created = DateTime.UtcNow;
        StateUpdated = DateTime.UtcNow;
        State = State.New;
        Tags = tags;
    }

    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime StateUpdated { get; set; }
    [Required]
    public State State { get; set; }
    public ICollection<Tag>? Tags { get; set; }
}
