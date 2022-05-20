namespace eruditionis.Database
{
    using eruditionis.Database.Models;
    using Microsoft.EntityFrameworkCore;

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
            }); 
            builder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id);
            }); 
            builder.Entity<Document>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            builder.Entity<Chat>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            builder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

    }

}
