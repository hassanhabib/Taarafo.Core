// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.GroupPosts;

namespace Taarafo.Core.Services.Foundations.GroupPosts
{
    public partial class GroupPostService : IGroupPostService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public GroupPostService(IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<GroupPost> AddGroupPostAsync(GroupPost groupPost) =>
        TryCatch(async () =>
        {
            ValidateGroupPostOnAdd(groupPost);

            return await this.storageBroker.InsertGroupPostAsync(groupPost);
        });
    }
}
