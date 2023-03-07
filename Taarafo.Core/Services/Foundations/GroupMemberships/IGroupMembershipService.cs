// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Taarafo.Core.Models.GroupMemberships;

namespace Taarafo.Core.Services.Foundations.GroupMemberships
{
    public interface IGroupMembershipService
    {
        ValueTask<GroupMembership> AddGroupMembershipAsync(GroupMembership groupMembership);
        ValueTask<GroupMembership> RetrieveGroupMembershipByIdAsync(Guid groupMembershipId);
    }
}