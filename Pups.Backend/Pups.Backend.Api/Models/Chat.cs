using System;
using System.Collections.Generic;

namespace Pups.Backend.Api.Models
{
    public partial class Chat
    {
        public Chat()
        {
            Messages = new HashSet<Message>();
        }

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TypeId { get; set; }

        public virtual ChatType Type { get; set; } = null!;
        public virtual ICollection<Message> Messages { get; set; }
    }
}
