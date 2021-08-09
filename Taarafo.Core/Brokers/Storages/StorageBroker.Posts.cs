// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Taarafo.Core.Models.Posts;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Post> Posts { get; set; }

        public async ValueTask<Post> InsertPostAsync(Post post)
        {
            EntityEntry<Post> postEntityEntry = await this.Posts.AddAsync(post);

            await this.SaveChangesAsync();

            return postEntityEntry.Entity;

        }

        public async ValueTask<Post> UpdatePostAsync(Post post)
        {
            EntityEntry<Post> postEntityEntry = this.Posts.Update(post);

            await this.SaveChangesAsync();

            return postEntityEntry.Entity;
        }

        public async ValueTask<Post> SelectPostByIdAsync(Guid postId)
        {
            this.ChangeTracker.QueryTrackingBehavior =
                QueryTrackingBehavior.NoTracking;

            return await Posts.FindAsync(postId);
        }

        public IQueryable<Post> SelectAllPosts() => this.Posts;
        public async ValueTask<Post> DeletePostAsync(Post post)
        {
            EntityEntry<Post> postEntityEntry = this.Posts.Remove(post);

            await this.SaveChangesAsync();

            return postEntityEntry.Entity;

        }
    }
}
