// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.Groups;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Group> Groups { get; set; }

        public async ValueTask<Group> InsertGroupAsync(Group group) =>
            await InsertAsync(group);

        public IQueryable<Group> SelectAllGroups() =>
            SelectAll<Group>();

        public async ValueTask<Group> SelectGroupByIdAsync(Guid groupId) =>
            await SelectAsync<Group>(groupId);

        public async ValueTask<Group> UpdateGroupAsync(Group group) =>
            await UpdateAsync(group);

        public async ValueTask<Group> DeleteGroupAsync(Group group) =>
            await DeleteAsync(group);
    }
}
