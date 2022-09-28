using Assignment3.Entities;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;

namespace Assignment3
{
    public class KanbanContextFactory : IDesignTimeDbContextFactory<KanbanContext>
    {
        public KanbanContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddUserSecrets<KanbanContext>().Build();
            var connectionString = configuration.GetConnectionString("Kanban");

            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
            optionsBuilder.UseNpgsql(connectionString);

            var context = new KanbanContext(optionsBuilder.Options);

            context.Tags.Include(t => t.Tasks).ToList();
            context.Users.Include(u => u.Tasks).ToList();
            context.Tasks.Include(t => t.Tags).ToList();
            return context;
        }
    }
}
