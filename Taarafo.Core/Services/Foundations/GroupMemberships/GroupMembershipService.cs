// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Taarafo.Core.Brokers.DateTimes;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.GroupMemberships;

namespace Taarafo.Core.Services.Foundations.GroupMemberships
{
    public class GroupMembershipService : IGroupMembershipService
    {
        private IStorageBroker storageBroker;
        private IDateTimeBroker dateTimeBroker;
        private ILoggingBroker loggingBroker;

        public GroupMembershipService(IStorageBroker storageBroker, IDateTimeBroker dateTimeBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<GroupMembership> AddGroupMembershipAsync(GroupMembership groupMembership) =>
            await this.storageBroker.InsertGroupMembershipAsync(groupMembership);
    }
}