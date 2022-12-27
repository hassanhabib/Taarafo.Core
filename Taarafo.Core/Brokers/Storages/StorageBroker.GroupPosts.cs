﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Taarafo.Core.Models.GroupPosts;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<GroupPost> GroupPosts { get; set; }

        public async ValueTask<GroupPost> InsertGroupPostAsync(GroupPost groupPost)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<GroupPost> groupPostEntityEntry =
                await broker.GroupPosts.AddAsync(groupPost);

            await broker.SaveChangesAsync();

            return groupPostEntityEntry.Entity;
        }

        public async ValueTask<GroupPost> SelectGroupPostByIdAsync(Guid groupPostId)
        {
            using var broker =
                 new StorageBroker(this.configuration);

            return await broker.GroupPosts.FindAsync(groupPostId);
        }

        public async ValueTask<GroupPost> DeleteGroupPostAsync(GroupPost groupPost)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<GroupPost> groupPostEntityEntry =
                broker.GroupPosts.Remove(groupPost);

            await broker.SaveChangesAsync();

            return groupPostEntityEntry.Entity;
        }
    }
}
