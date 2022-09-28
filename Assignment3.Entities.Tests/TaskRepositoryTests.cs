using Assignment3.Core;

namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture dbFixture;

    public TaskRepositoryTests(DatabaseFixture dbFixture)
    {
        this.dbFixture = dbFixture;
    }

    [Fact]
    public void Create_Task_Upholds_Rules()
    {
        //Arrange
        var taskDTO = new TaskCreateDTO("UI Layout", null, "Redo design of ui layout", new List<string>() { "something", "somethign else"});


        //Act
        var (response, taskid) = dbFixture.TaskRepository.Create(taskDTO);
        var task = dbFixture.Context.Tasks.Find(taskid);

        //Assert
        Assert.Equal(Response.Created, response);
        Assert.Equal(State.New, task.State);
        Assert.Equal(DateTime.UtcNow, task.Created, precision: TimeSpan.FromSeconds(10));
        Assert.Equal(DateTime.UtcNow, task.StateUpdated, precision: TimeSpan.FromSeconds(10));
    }

    [Fact]
    public void Delete_New()
    {
        //Arrange
        var task = new Task("UI Layout", null, "Redo design of ui layout", null);
        dbFixture.Context.Tasks.Add(task);
        dbFixture.Context.SaveChanges();

        //Act
        var response = dbFixture.TaskRepository.Delete(task.Id);
        task = dbFixture.Context.Tasks.Find(task.Id);

        //Assert
        Assert.Equal(Response.Deleted, response);
        Assert.Null(task);
    }

    [Fact]
    public void Delete_Active_HaveState_Removed()
    {
        //Arrange
        var task = new Task("Database Structure", null, "Setup database with suituble data structures", null);
        task.State = State.Active;
        dbFixture.Context.Tasks.Add(task);
        dbFixture.Context.SaveChanges();

        //Act
        var response = dbFixture.TaskRepository.Delete(task.Id);

        //Assert
        Assert.Equal(Response.Deleted, response);
        Assert.Equal(State.Removed, task.State);
    }

    [Fact]
    public void Delete_Return_Conflict()
    {
        //Arrange
        var task = new Task("Database Structure", null, "Setup database with suituble data structures", null);
        task.State = State.Closed;
        dbFixture.Context.Tasks.Add(task);
        dbFixture.Context.SaveChanges();

        //Act
        var response = dbFixture.TaskRepository.Delete(task.Id);

        //Assert
        Assert.Equal(Response.Conflict, response);
    }

    [Fact]
    public void Create_Task_With_NonExsistingUser_Return_BadRequest()
    {
        //Arrange
        var taskDTO = new TaskCreateDTO("UI Layout", 7, "Redo design of ui layout", null);

        //Act
        var (response, taskId) = dbFixture.TaskRepository.Create(taskDTO);

        //Assert
        Assert.Equal(Response.BadRequest, response);
    }

    [Fact]
    public void Changing_State_Updates_StateUpdated()
    {
        //Arrange
        var task = new Task("Database Structure", null, "Setup database with suituble data structures", null);
        task.StateUpdated = DateTime.UtcNow - TimeSpan.FromMinutes(1);
        dbFixture.Context.Tasks.Add(task);
        dbFixture.Context.SaveChanges();

        var updateDTO = new TaskUpdateDTO(task.Id, task.Title, task.UserId, task.Description, null, State.Resolved);

        //Act
        var response = dbFixture.TaskRepository.Update(updateDTO);

        //Assert

        Assert.Equal(Response.Updated, response);
        Assert.Equal(State.Resolved, task.State);
        Assert.Equal(DateTime.UtcNow, task.StateUpdated, precision: TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void Link_Tags_To_Task()
    {
        //Arrange
        var tag = new Tag("Tag1");
        dbFixture.Context.Tags.Add(tag);
        dbFixture.Context.SaveChanges();

        var taskDTO = new TaskCreateDTO("UI Layout", null, "Redo design of ui layout", new List<string>() { "Tag1" });

        //Act
        var (response, taskId) = dbFixture.TaskRepository.Create(taskDTO);
        var task = dbFixture.Context.Tasks.Find(taskId);

        //Assert
        Assert.Equal(Response.Created, response);
        Assert.True(task.Tags.FirstOrDefault().Id == tag.Id);
    }

    [Fact]
    public void Update_NonExsistingTask_Return_NotFound()
    {
        //Arrange
        var updateDTO = new TaskUpdateDTO(7, "UI Layout", null, "Redo design of ui layout", null, State.Resolved);

        //Act
        var response = dbFixture.TaskRepository.Update(updateDTO);

        //Assert
        Assert.Equal(Response.NotFound, response);
    }

    [Fact]
    public void Read_NonTexsistingTask_Return_Null()
    {
        //Arrange

        //Act
        var task = dbFixture.TaskRepository.Read(7);

        //Assert
        Assert.Null(task);
    }
}
