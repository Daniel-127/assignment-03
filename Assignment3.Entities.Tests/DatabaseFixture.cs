using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3.Entities.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public UserRepository UserRepository { get; private set; }
        public TagRepository TagRepository { get; private set; }
        public TaskRepository TaskRepository { get; private set; }

        public KanbanContext Context { get; init; }

        public DatabaseFixture()
        {
            var factory = new KanbanContextFactory();
            Context = factory.CreateDbContext(null);
            TaskRepository = new TaskRepository(Context);
        }
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
