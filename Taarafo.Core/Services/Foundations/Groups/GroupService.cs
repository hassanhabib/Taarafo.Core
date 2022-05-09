// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Brokers.DateTimes;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Groups;

namespace Taarafo.Core.Services.Foundations.Groups
{
    public partial class GroupService : IGroupService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public GroupService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Group> CreateGroupAsync(Group group) =>
        TryCatch(async () =>
        {
            ValidateGroupOnAdd(group);

            return await this.storageBroker.InsertGroupAsync(group);
        });

        public IQueryable<Group> RetrieveAllGroups() =>
        TryCatch(() => this.storageBroker.SelectAllGroups());

        public ValueTask<Group> UpdateGroupAsync(Group group)=>
        TryCatch(async () =>
        {
            ValidateGroupOnModify(group);

            var maybeGroup =
                await this.storageBroker.SelectGroupByIdAsync(group.Id);

            ValidateStorageGroup(maybeGroup, group.Id);

            return await this.storageBroker.UpdateGroupAsync(group);
        });
    }
}
