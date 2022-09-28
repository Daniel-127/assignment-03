namespace Assignment3.Entities.Tests;
using Assignment3.Entities;
using Assignment3.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class TagRepositoryTests
{
  TagRepository tagRepository;
  KanbanContext context;
  TaskRepository taskRepository;

  public TagRepositoryTests() {
    context = (new KanbanContextFactory()).CreateDbContext(null);
    tagRepository = new TagRepository(context);
    taskRepository = new TaskRepository(context);
  }

  [Fact]
  public void create_tag_and_adds_to_db() {
    var (res, id) = tagRepository.Create(new TagCreateDTO("tag-navn-1"));
    Assert.Equal(Response.Created, res);
    Assert.NotNull(context.Tags.Find(id));
  }

  [Fact]
  public void create_duplicate_tag_return_conflict() {
    var (res1, id1) = tagRepository.Create(new TagCreateDTO("tag-navn-1"));
    var (res2, id2) = tagRepository.Create(new TagCreateDTO("tag-navn-1"));

    Assert.Equal(Response.Created, res1);
    Assert.Equal(1, id1);

    Assert.Equal(Response.Conflict, res2);
    Assert.Equal(-1, id2);
  }

  [Fact]
  public void delete_tag_not_in_use_without_force_returns_Deleted(){
    var (res, id) = tagRepository.Create(new TagCreateDTO("tag-navn-1"));
    var deleteResponse = tagRepository.Delete(id, false);
    Assert.Equal(Response.Deleted, deleteResponse);
  }

  [Fact]
  public void delete_tag_not_in_use_with_force_returns_Deleted(){
    var (res, id) = tagRepository.Create(new TagCreateDTO("tag-navn-1"));
    var deleteResponse = tagRepository.Delete(id, true);
    Assert.Equal(Response.Deleted, deleteResponse);
  }

  [Fact]
  public void delete_tag_in_use_with_force_returns_Deleted(){
    var (tagRes, tagId) = tagRepository.Create(new TagCreateDTO("tag-navn-1"));
    var taskDTO = new TaskCreateDTO("UI Layout", null, "Redo design of ui layout", new List<String>{"tag-navn-1"});
    var (taskRes, taskId) = taskRepository.Create(taskDTO);

    var insertedTag = context.Tags.Find(tagId);
    var insertedTask = context.Tasks.Find(taskId);
    bool insertedTagContainsInsertedTask = insertedTag.Tasks.Contains(insertedTask);

    Assert.NotNull(insertedTag);
    Assert.NotNull(insertedTask);
    Assert.Equal(true, insertedTagContainsInsertedTask);

    var deleteResponse = tagRepository.Delete(tagId, true);

    Assert.Equal(Response.Deleted, deleteResponse);
  }

  [Fact]
  public void delete_tag_in_use_without_force_returns_Conflict(){
    var (tagRes, tagId) = tagRepository.Create(new TagCreateDTO("tag-navn-1"));
    var taskDTO = new TaskCreateDTO("UI Layout", null, "Redo design of ui layout", new List<String>{"tag-navn-1"});
    var (taskRes, taskId) = taskRepository.Create(taskDTO);

    var insertedTag = context.Tags.Find(tagId);
    var insertedTask = context.Tasks.Find(taskId);
    bool insertedTagContainsInsertedTask = insertedTag.Tasks.Contains(insertedTask);

    Assert.NotNull(insertedTag);
    Assert.NotNull(insertedTask);
    Assert.Equal(true, insertedTagContainsInsertedTask);

    var deleteResponse = tagRepository.Delete(tagId, false);

    Assert.Equal(Response.Conflict, deleteResponse);
  }

  [Fact]
  public void find_existing_tag_returns_tagdto() {
    var (tagRes, tagId) = tagRepository.Create(new TagCreateDTO("tag-navn-1"));
    var result = tagRepository.Find(tagId);
    Assert.NotNull(result);
    Assert.Equal("tag-navn-1", result.Name);
  }

  [Fact]
  public void find_non_existing_tag_returns_null() {
    var result = tagRepository.Find(1);
    Assert.Null(result);
  }

  [Fact]
  public void update_existing_tag_returns_Updated(){
    var (res, id) = tagRepository.Create(new TagCreateDTO("tag-navn-1"));
    var updateResponse = tagRepository.Update(new TagUpdateDTO(id, "nyt-tag-navn-1"));

    var updatedName = context.Tags.Find(id).Name;
    Assert.Equal("nyt-tag-navn-1", updatedName);
    Assert.Equal(Response.Updated, updateResponse);
  }

  [Fact]
  public void update_nonexisting_tag_returns_Updated(){
    var updateResponse = tagRepository.Update(new TagUpdateDTO(1, "nyt-tag-navn-1"));

    Assert.Equal(Response.NotFound, updateResponse);
  }

}
