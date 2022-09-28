using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    private readonly KanbanContext context;

    public TaskRepository(KanbanContext context)
    {
        this.context = context;
    }

    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        var tagsDB = context.Tags.ToList();
        var tags = tagsDB.Where(tag => task.Tags.Contains(tag.Name)).ToList();
        var dbTask = new Task(task.Title, task.AssignedToId, task.Description, tags);
        context.Tasks.Add(dbTask);
        context.SaveChanges();
        return (Response.Created, dbTask.Id);
    }

    public Response Delete(int taskId)
    {
        var task = context.Tasks.Find(taskId);
        if (task == null) return Response.NotFound;
        switch (task.State)
        {
            case State.Resolved:
            case State.Closed:
            case State.Removed:
                return Response.Conflict;
            case State.Active:
                task.State = State.Removed;
                context.SaveChanges();
                return Response.Deleted;
            case State.New:
                context.Tasks.Remove(task);
                context.SaveChanges();
                return Response.Deleted;
            default: 
                return Response.BadRequest;
        }
    }

    public TaskDetailsDTO Read(int taskId)
    {
        var task = context.Tasks.Find(taskId);
        if (task == null) return null!;
        var tagNames = task.Tags.Select(tag => tag.Name).ToList();
        var taskDetailsDTO = new TaskDetailsDTO(task.Id, task.Title, task.Description!, task.Created, task.User!.Name!, tagNames, task.State, task.StateUpdated);
        return taskDetailsDTO;
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        return context.Tasks.Select(task => new TaskDTO(task.Id, task.Title, task.User!.Name!, TagNames(task), task.State)).ToList();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        return ReadAll().Where(task => task.State == state).ToList();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
       return ReadAll().Where(task => task.Tags.Contains(tag)).ToList();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        return context.Tasks.Where(task => task.UserId == userId).Select(task => new TaskDTO(task.Id, task.Title, task.User!.Name!, TagNames(task), task.State)).ToList();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        return ReadAllByState(State.Removed);
    }

    public Response Update(TaskUpdateDTO task)
    {
        var dbTask = context.Tasks.Find(task.Id);
        if (dbTask == null) return Response.NotFound;
        dbTask.Title = task.Title;
        dbTask.Description = task.Description;
        dbTask.Tags = context.Tags.Join(task.Tags, tag => tag.Name, name => name, (tag, name) => tag).ToList();
        if (dbTask.UserId != task.AssignedToId)
        {
            dbTask.UserId = task.AssignedToId;
            dbTask.User = null;
        }
        if (dbTask.State != task.State)
        {
            dbTask.State = task.State;
            dbTask.StateUpdated = DateTime.UtcNow;
        }
        context.SaveChanges();
        return Response.Updated;
    }

    private List<string> TagNames(Task task)
    {
        return task.Tags.Select(tag => tag.Name).ToList();
    }
}
