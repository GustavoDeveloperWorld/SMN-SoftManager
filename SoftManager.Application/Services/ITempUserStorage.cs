using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftManager.Application.Services
{
    public interface ITempUserStorage
    {
        Task StoreTempUserAsync(string email, string password);
        Task<TempUser> RetrieveTempUserAsync(string email);
    }

    public class TempUserStorage : ITempUserStorage
    {
        private readonly Dictionary<string, TempUser> _tempUsers = new Dictionary<string, TempUser>();

        public Task StoreTempUserAsync(string email, string password)
        {
            var tempUser = new TempUser { Email = email, Password = password };
            _tempUsers[email] = tempUser;
            return Task.CompletedTask;
        }

        public Task<TempUser> RetrieveTempUserAsync(string email)
        {
            _tempUsers.TryGetValue(email, out var tempUser);
            return Task.FromResult(tempUser);
        }
    }

    public class TempUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }


}
