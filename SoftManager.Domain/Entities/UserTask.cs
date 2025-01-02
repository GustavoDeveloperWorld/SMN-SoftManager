using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SoftManager.Domain.Entities
{
    public class UserTask
    {
        public Guid Id { get; set; }
        public string? Task { get; set; }
        public string? Message { get; set; }
        public DateTime DueDate { get; set; } 
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}
