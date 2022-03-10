// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Taarafo.Core.Models.Groups;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Group> Groups { get; set; }

        public async ValueTask<Group> InsertGroupAsync(Group group)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Group> groupEntityEntry =
                await broker.Groups.AddAsync(group);

            await broker.SaveChangesAsync();

            return groupEntityEntry.Entity;
        }

        public IQueryable<Group> SelectAllGroups()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.Groups;
        }

        public async ValueTask<Group> SelectGroupByIdAsync(Guid groupId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.Groups.FindAsync(groupId);
        }

        public async ValueTask<Group> DeleteGroupAsync(Group group)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Group> groupEntityEntry =
                broker.Groups.Remove(group);

            await broker.SaveChangesAsync();

            return groupEntityEntry.Entity;
        }
    }
}
