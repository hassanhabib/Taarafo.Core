// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Taarafo.Core.Models.GroupPosts;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<GroupPost> GroupPosts { get; set; }

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
