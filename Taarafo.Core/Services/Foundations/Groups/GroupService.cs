// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Groups;

namespace Taarafo.Core.Services.Foundations.Groups
{
    public partial class GroupService : IGroupService
    {
        private readonly IStorageBroker storageBroker;

        public GroupService(IStorageBroker storageBroker) =>
            this.storageBroker = storageBroker;

        public IQueryable<Group> RetrieveAllGroups()
        {
            throw new System.NotImplementedException();
        }
    }
}
