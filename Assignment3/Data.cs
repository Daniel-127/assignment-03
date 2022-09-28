using Assignment3.Entities;
using Microsoft.EntityFrameworkCore;
using Task = Assignment3.Entities.Task;

namespace Assignment3
{
    internal class Data
    {
        public static void Seed(KanbanContext context)
        {
            /*context.Database.ExecuteSqlRaw(TruncateString("Tags"));
            context.Database.ExecuteSqlRaw(TruncateString("Users"));
            context.Database.ExecuteSqlRaw(TruncateString("Tasks"));
            context.Database.ExecuteSqlRaw(TruncateString("TagTask"));
            context.SaveChanges();*/

            var frontend = new Tag("Frontend");
            var database = new Tag("Database");
            var algorithmic = new Tag("Algorithmic");

            /*var frontend = context.Tags.Where(tag => tag.Name == "Frontend").FirstOrDefault();
            var database = context.Tags.Where(tag => tag.Name == "Database").FirstOrDefault();
            var algorithmic = context.Tags.Where(tag => tag.Name == "Algorithmic").FirstOrDefault();*/

            var lukas = new User("Lukas", "lukas@mail.com");
            var mathias = new User("Mathias", "mathias@mail.com");
            var silas = new User("Silas", "silas@mail.com");


            context.Tags.AddRange(frontend, database, algorithmic);
            context.Users.AddRange(lukas, mathias, silas);
            context.SaveChanges();


            var layout = new Task("UI Layout", lukas.Id, "Redo design of ui layout", new List<Tag>(){ frontend });
            var databaseStructure = new Task("Database Structure", mathias.Id, "Setup database with suituble data structures", new List<Tag>(){ database, algorithmic });
            var userInfo = new Task("User Info", silas.Id, "Get and display user info on profile", new List<Tag>() { database, frontend });

            context.Tasks.AddRange(layout, databaseStructure, userInfo);

            context.SaveChanges();
        }

        private static string TruncateString(string table)
        {
            return $"TRUNCATE TABLE \"{table}\" CASCADE";
        }
    }
}
