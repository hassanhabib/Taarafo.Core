﻿// ---------------------------------------------------------------
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
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Post> postEntityEntry =
                await broker.Posts.AddAsync(post);

            await broker.SaveChangesAsync();

            return postEntityEntry.Entity;
        }

        public IQueryable<Post> SelectAllPosts()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.Posts;
        }

        public async ValueTask<Post> SelectPostByIdAsync(Guid postId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            broker.ChangeTracker.QueryTrackingBehavior = 
                QueryTrackingBehavior.NoTracking;

            return await broker.Posts.FindAsync(postId);
        }

        public async ValueTask<Post> UpdatePostAsync(Post post)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Post> postEntityEntry =
                broker.Posts.Update(post);

            await broker.SaveChangesAsync();

            return postEntityEntry.Entity;
        }

        public async ValueTask<Post> DeletePostAsync(Post post)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Post> postEntityEntry =
                broker.Posts.Remove(post);

            await broker.SaveChangesAsync();

            return postEntityEntry.Entity;
        }
    }
}
