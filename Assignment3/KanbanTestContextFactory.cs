using Assignment3.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    public class KanbanTestContextFactory
    {
        public KanbanContext CreateDbContext()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection);

            var context = new KanbanContext(builder.Options);
            context.Database.EnsureCreated();
            context.SaveChanges();


            context.Tags.Include(t => t.Tasks).ToList();
            context.Users.Include(u => u.Tasks).ToList();
            context.Tasks.Include(t => t.Tags).ToList();

            return context;
        }
    }
}
