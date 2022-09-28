namespace Assignment3.Entities;

public class User
{
    public User(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public int Id { get; set; }
    [Required, StringLength(100)]
    public string Name { get; set; }
    [Required, StringLength(100)]
    public string Email { get; set; }
    public List<Task>? Tasks { get; set; }
}
