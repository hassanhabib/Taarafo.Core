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
using Taarafo.Core.Models.Posts;

namespace Taarafo.Core.Services.Foundations.Posts
{
    public class PostService : IPostService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public PostService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<Post> AddPostAsync(Post post)
        {
            return await this.storageBroker.InsertPostAsync(post);
        }

        public async ValueTask<Post> RetrievePostByIdAsync(Guid postId)
        {
            return await this.storageBroker.SelectPostByIdAsync(postId);
        }

        public IQueryable<Post> RetrieveAllPosts()
        {
            return this.storageBroker.SelectAllPosts();
        }

        public async ValueTask<Post> ModifyPostAsync(Post post)
        {
            return await this.storageBroker.UpdatePostAsync(post);
        }

        public async ValueTask<Post> RemovePostByIdAsync(Guid postId)
        {
            Post post = await this.storageBroker.SelectPostByIdAsync(postId);

            return await this.storageBroker.DeletePostAsync(post);
        }
    }
}
