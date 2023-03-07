// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Taarafo.Core.Brokers.DateTimes;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.GroupMemberships;

namespace Taarafo.Core.Services.Foundations.GroupMemberships
{
    public partial class GroupMembershipService : IGroupMembershipService
    {
        private IStorageBroker storageBroker;
        private IDateTimeBroker dateTimeBroker;
        private ILoggingBroker loggingBroker;

        public GroupMembershipService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<GroupMembership> AddGroupMembershipAsync(GroupMembership groupMembership) =>
            TryCatch(async () =>
            {
                ValidateGroupMembershipOnAdd(groupMembership);

                return await this.storageBroker.InsertGroupMembershipAsync(groupMembership);
            });

        public ValueTask<GroupMembership> RetrieveGroupMembershipByIdAsync(Guid groupMembershipId)
        {
            throw new NotImplementedException();
        }
    }
}