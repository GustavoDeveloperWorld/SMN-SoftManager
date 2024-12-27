using SoftManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftManager.Domain.Interfaces
{
    public interface IUserTaskRepository : IRepository<UserTask>
    {
        Task<IEnumerable<UserTask>> GetTasksByUserIdAsync(Guid userId);
        Task<IEnumerable<UserTask>> GetPendingTasksAsync();
    }
}
