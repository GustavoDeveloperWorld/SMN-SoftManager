using Microsoft.EntityFrameworkCore;
using SoftManager.Domain.Entities;
using SoftManager.Domain.Interfaces;
using SoftManager.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftManager.Infrastructure.Repositories
{
    public class UserTaskRepository : Repository<UserTask>, IUserTaskRepository
    {
        public UserTaskRepository(InfraDbContext context) : base(context) { }

        public async Task<IEnumerable<UserTask>> GetTasksByUserIdAsync(Guid userId)
        {
            return await _context.UserTasks
                .Where(task => task.Id == userId)
                .ToListAsync();
        }
        public async Task<IEnumerable<UserTask>> GetPendingTasksAsync()
        {
            return await _context.Set<UserTask>()
                .Where(task => !task.IsCompleted)
                .ToListAsync();
        }
    }
}
