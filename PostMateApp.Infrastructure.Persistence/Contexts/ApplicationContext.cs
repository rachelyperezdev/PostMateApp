using Microsoft.EntityFrameworkCore;
using PostMateApp.Core.Domain.Common;
using PostMateApp.Core.Domain.Entities;

namespace PostMateApp.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        DbSet<Friendship> Friendships { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Reply> Replies { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.Modified = DateTime.Now;
                        entry.Entity.LastModifiedBy = "DefaultAppUser";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Tables
            modelBuilder.Entity<Friendship>().ToTable("Friendships");
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Reply>().ToTable("Replies");
            #endregion

            #region "Primary Keys"
            modelBuilder.Entity<Friendship>().HasKey(f => f.Id);
            modelBuilder.Entity<Post>().HasKey(p => p.Id);
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Reply>().HasKey(r => r.Id);
            #endregion

            #region Relationships
            modelBuilder.Entity<Post>()
                .HasMany<Comment>(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasMany<Reply>(c => c.Replies)
                .WithOne(r => r.Comment)
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region "Property Configurations"
            #region Friends
            modelBuilder.Entity<Friendship>()
                .Property(f => f.ProfileOwnerId)
                .IsRequired();

            modelBuilder.Entity<Friendship>()
                .Property(f => f.FriendId)
                .IsRequired();
            #endregion
            #region Posts
            modelBuilder.Entity<Post>()
                .Property (p => p.UserId)
                .IsRequired();

            modelBuilder.Entity<Post>()
                .Property(p => p.PublicationDate)
                .IsRequired();
            #endregion
            #region Comments
            modelBuilder.Entity<Comment>()
                .Property(c => c.UserId)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .Property(c => c.Text) 
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .Property(c => c.PostId)
                .IsRequired();
            #endregion
            #region Replies
            modelBuilder.Entity<Reply>()
                .Property(r => r.CommentId) 
                .IsRequired();

            modelBuilder.Entity<Reply>()
                .Property(r => r.Text) 
                .IsRequired();
            #endregion
            #endregion
        }
    }
}
