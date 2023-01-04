// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.GroupMemberships;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<GroupMembership> GroupMemberships { get; set; }

        public async ValueTask<GroupMembership> InsertGroupMembershipAsync(GroupMembership groupMembership) =>
            await InsertAsync(groupMembership);

        public IQueryable<GroupMembership> SelectAllGroupMemberships() =>
            SelectAll<GroupMembership>();

        public async ValueTask<GroupMembership> SelectGroupMembershipByIdAsync(Guid id) =>
           await SelectAsync<GroupMembership>(id);

        public async ValueTask<GroupMembership> UpdateGroupMembershipAsync(GroupMembership groupMembership) =>
            await UpdateAsync(groupMembership);

        public async ValueTask<GroupMembership> DeleteGroupMembershipAsync(GroupMembership groupMembership) =>
           await DeleteAsync(groupMembership);
    }
}