namespace Assignment3.Entities.Tests;
using Assignment3.Entities;
using Assignment3.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class TagRepositoryTests
{
  TagRepository repository;
  KanbanContext context;

  public TagRepositoryTests() {
    context = (new KanbanContextFactory()).CreateDbContext(null);
    repository = new TagRepository(context);
  }

  [Fact]
  public void create_tag_and_adds_to_db() {
    var (res, id) = repository.Create(new TagCreateDTO("tag-navn-1"));
    Assert.Equal(Response.Created, res);
    Assert.NotNull(context.Tags.Find(id));
  }

  [Fact]
  public void create_duplicate_tag_return_conflict() {
    var (res1, id1) = repository.Create(new TagCreateDTO("tag-navn-1"));
    var (res2, id2) = repository.Create(new TagCreateDTO("tag-navn-1"));

    Assert.Equal(Response.Created, res1);
    Assert.Equal(1, id1);

    Assert.Equal(Response.Conflict, res2);
    Assert.Equal(-1, id2);
  }
}
