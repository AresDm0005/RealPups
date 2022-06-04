using System;
using System.Collections.Generic;

namespace Pups.Backend.Api.Models
{
    public partial class User
    {
        public User()
        {
            Messages = new HashSet<Message>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Info { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastSeen { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
