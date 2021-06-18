// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Posts;

namespace Taarafo.Core.Services.Foundations.Posts
{
    public class PostService : IPostService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public PostService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public IQueryable<Post> RetrieveAllPosts()
        {
            IQueryable<Post> storagePosts = this.storageBroker.SelectAllPosts();

            return storagePosts;
        }
    }
}
