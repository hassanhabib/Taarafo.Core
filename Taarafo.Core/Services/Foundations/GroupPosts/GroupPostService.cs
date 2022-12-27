// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Taarafo.Core.Brokers.DateTimes;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.GroupPosts;

namespace Taarafo.Core.Services.Foundations.GroupPosts
{
    public class GroupPostService : IGroupPostService
    {
        private IStorageBroker storageBroker;
        private IDateTimeBroker dateTimeBroker;
        private ILoggingBroker loggingBroker;

        public GroupPostService(IStorageBroker storageBroker, IDateTimeBroker dateTimeBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<GroupPost> RetrieveGroupPostByIdAsync(Guid groupPostId)
        {
            throw new NotImplementedException();
        }
    }
}