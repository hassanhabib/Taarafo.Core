// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Posts;

namespace Taarafo.Core.Services.Foundations.Posts
{
    public partial class PostService : IPostService
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

        public async ValueTask<Post> AddPostAsync(Post post) =>
            await this.storageBroker.InsertPostAsync(post);

        public IQueryable<Post> RetrieveAllPosts() =>
        TryCatch(() => this.storageBroker.SelectAllPosts());
    }
}
