using SoftManager.Domain.Entities;
using SoftManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftManager.Application.Services
{
    public class UserService
    {
        private readonly IUserTaskRepository _userTaskRepository;

        public UserService(IUserTaskRepository userTaskRepository)
        {
            _userTaskRepository = userTaskRepository;
        }

        public async Task<IEnumerable<UserTask>> GetTasksForUserAsync(Guid userId)
        {
            return await _userTaskRepository.GetTasksByUserIdAsync(userId);
        }
    }
}