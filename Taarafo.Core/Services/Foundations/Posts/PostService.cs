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
    public partial class PostService : IPostService
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

        public ValueTask<Post> AddPostAsync(Post post) =>
        TryCatch(async () =>
        {
            ValidatePost(post);

            return await this.storageBroker.InsertPostAsync(post);
        });

        public ValueTask<Post> RetrievePostByIdAsync(Guid postId) =>
            TryCatch(async () =>
            {
                ValidatePostById(postId);
                Post storagePost = await this.storageBroker.SelectPostByIdAsync(postId);
                ValiateStoragePost(storagePost,postId);

                return storagePost;
            });

        public IQueryable<Post> RetrieveAllPosts() =>
        TryCatch(() => this.storageBroker.SelectAllPosts());

        public ValueTask<Post> RemovePostByIdAsync(Guid postId) =>
        TryCatch( async () =>
        {    
            ValidatePostById(postId);
            Post maybePost = await this.storageBroker
                .SelectPostByIdAsync(postId);

            ValiateStoragePost(maybePost, postId);

            return await this.storageBroker
                .DeletePostAsync(maybePost);
        });
    }
}
