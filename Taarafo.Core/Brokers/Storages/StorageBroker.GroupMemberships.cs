// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
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

        public async ValueTask<GroupMembership> SelectGroupMembershipByIdAsync(Guid id) =>
           await SelectAsync<GroupMembership>(id);
    }
}