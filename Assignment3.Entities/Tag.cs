namespace Assignment3.Entities;

public class Tag
{
    public Tag(string name)
    {
        Name = name;
        Tasks = new List<Task>();
    }

    public int Id { get; set; }
    [StringLength(50), Required]
    public string Name { get; set; }
    public List<Task> Tasks { get; set; }
}
