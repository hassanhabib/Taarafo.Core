// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using Taarafo.Core.Models.Groups;

namespace Taarafo.Core.Services.Foundations.Groups
{
    public interface IGroupService
    {
        IQueryable<Group> RetrieveAllGroups();
    }
}
