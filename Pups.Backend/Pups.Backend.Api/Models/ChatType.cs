using System;
using System.Collections.Generic;

namespace Pups.Backend.Api.Models
{
    public partial class ChatType
    {
        public ChatType()
        {
            Chats = new HashSet<Chat>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Chat> Chats { get; set; }
    }
}
