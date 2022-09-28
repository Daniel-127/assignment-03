using Microsoft.EntityFrameworkCore;

var factory = new KanbanContextFactory();
using var context = factory.CreateDbContext(args);

Data.Seed(context);

/*var user = context.Users.Where(user => user.Name == "Mathias").FirstOrDefault();
foreach (var task in user.Tasks)
{
    Console.WriteLine(task.Title);
}*/