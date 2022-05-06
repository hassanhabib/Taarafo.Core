// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Groups;

namespace Taarafo.Core.Services.Foundations.Groups
{
    public interface IGroupService
    {
        ValueTask<Group> CreateGroupAsync(Group group);
        IQueryable<Group> RetrieveAllGroups();
        ValueTask<Group> UpdateGroupAsync(Group group);
    }
}
