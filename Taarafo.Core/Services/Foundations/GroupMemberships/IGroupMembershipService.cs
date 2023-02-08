// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.GroupMemberships;

namespace Taarafo.Core.Services.Foundations.GroupMemberships
{
    public interface IGroupMembershipService
    {
        ValueTask<GroupMembership> AddGroupMembershipAsync(GroupMembership groupMembership);
        IQueryable<GroupMembership> RetrieveAllGroupMemberships();
    }
}