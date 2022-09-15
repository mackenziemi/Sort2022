using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sort2022.Data.Contracts
{
    public interface ITaskRepository
    {
        Task<IList<Sort2022.Data.Models.Task>> GetAll();

        Task<Models.Task> GetById(int id);

        Task<Models.Task> AddTask(Models.Task task);
    }
}
