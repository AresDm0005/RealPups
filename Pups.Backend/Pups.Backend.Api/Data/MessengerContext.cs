using Microsoft.EntityFrameworkCore;
using Pups.Backend.Api.Models;
using System.Reflection;

namespace Pups.Backend.Api.Data
{
    public partial class MessengerContext : DbContext
    {
        public MessengerContext()
        {
        }

        public MessengerContext(DbContextOptions<MessengerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<ChatMember> ChatMembers { get; set; } = null!;
        public virtual DbSet<ChatStatus> ChatStatuses { get; set; } = null!;
        public virtual DbSet<ChatType> ChatTypes { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());   
        }
    }
}
