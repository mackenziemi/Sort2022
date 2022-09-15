using Microsoft.EntityFrameworkCore;
using Sort2022.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sort2022.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly sort2022Context _context;

        public TaskRepository(sort2022Context context)
        {
            _context=context;   
        }

        public async Task<Models.Task> AddTask(Models.Task task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<IList<Models.Task>> GetAll()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<Models.Task> GetById(int id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);
        }
    }
}
