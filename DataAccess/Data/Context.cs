using _Models;
using _Models.RelationModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class Context : IdentityDbContext<IdentityUser>
    {
        public Context(DbContextOptions<Context> options): base (options)
        {
            
        }

        public DbSet<ApplicationUser> applicationUsers { get; private set; }
        public DbSet<Post> posts { get; private set; }
        public DbSet<User_Friend> user_Friends { get; private set; }
        public DbSet<FriendRequest> friendRequests { get; private set; }
        public DbSet<User_VoteUps> User_VoteUps { get; private set; }
        public DbSet<Comment> comments { get; private set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User_Friend>()
                .HasOne(x => x.User)
                .WithMany(y => y.Friends)
                .HasForeignKey(j => j.UserId);

            builder.Entity<User_Friend>()
                .HasOne(x => x.Friend)
                .WithMany(y => y.Users)
                .HasForeignKey(j => j.FriendId);

            builder.Entity<User_VoteUps>()
                .HasOne(x => x.Post)
                .WithMany(y => y.postsVotedUp)
                .HasForeignKey(j => j.postId);

            builder.Entity<User_VoteUps>()
                .HasOne(x => x.user)
                .WithMany(y => y.Voters)
                .HasForeignKey(j => j.UserId);


            builder.Entity<User_Posts>()
                .HasOne(x => x.Post)
                .WithMany(y => y.postsSaved)
                .HasForeignKey(j => j.postId);

            builder.Entity<User_Posts>()
                .HasOne(x => x.user)
                .WithMany(y => y.Saver)
                .HasForeignKey(j => j.UserId);
        }
    }
}
