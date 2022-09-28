namespace Assignment3.Entities;

using System.Collections.Generic;
using Assignment3.Core;

public class TagRepository : ITagRepository
{
  private readonly KanbanContext _context;

  public TagRepository(KanbanContext context) {
    _context = context;
  }

  public (Response Response, int TagId) Create(TagCreateDTO tag)
  {
    if(_context.Tags.Where(t => t.Name == tag.Name).FirstOrDefault() != null) {
        Console.WriteLine("LAUGE");
        return (Response.Conflict, -1); 
    }
    var t = new Tag(tag.Name);
    _context.Tags.Add(t);
    _context.SaveChanges();
    return (Response.Created, t.Id);
  }

  public Response Delete(int tagId, bool force = false)
  {
    throw new NotImplementedException();
  }

  public TagDTO Find(int tagId)
  {
    throw new NotImplementedException();
  }

  public IReadOnlyCollection<TagDTO> Read()
  {
    throw new NotImplementedException();
  }

    public TagDTO Read(int tagId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TagDTO> ReadAll()
    {
        throw new NotImplementedException();
    }

    public Response Update(TagUpdateDTO tag)
  {
    throw new NotImplementedException();
  }
}
