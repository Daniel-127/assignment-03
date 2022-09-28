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
        var taskDTO = new TaskCreateDTO("UI Layout", null, "Redo design of ui layout", null);


        //Act
        var (response, taskid) = dbFixture.TaskRepository.Create(taskDTO);
        var task = dbFixture.Context.Tasks.Find(taskid);


        Assert.Equal(Response.Created, response);
        Assert.Equal(State.New, task.State);
    }
}
