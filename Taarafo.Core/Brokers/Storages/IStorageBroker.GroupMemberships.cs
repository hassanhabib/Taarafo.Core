// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Taarafo.Core.Models.GroupMemberships;

namespace Taarafo.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<GroupMembership> InsertGroupMembershipAsync(GroupMembership groupMembership);
        ValueTask<GroupMembership> SelectGroupMembershipByIdAsync(Guid id);
        ValueTask<GroupMembership> UpdateGroupMembershipAsync(GroupMembership groupMembership);
    }
}