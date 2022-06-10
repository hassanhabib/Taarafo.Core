// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Groups;

namespace Taarafo.Core.Services.Foundations.Groups
{
    public interface IGroupService
    {
        ValueTask<Group> CreateGroupAsync(Group group);
        IQueryable<Group> RetrieveAllGroups();
        ValueTask<Group> RetrieveGroupByIdAsync(Guid groupId);
        ValueTask<Group> UpdateGroupAsync(Group group);
        ValueTask<Group> RemoveGroupByIdAsync(Guid groupId);
    }
}
