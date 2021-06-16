// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Posts;

namespace Taarafo.Core.Services.Foundations.Posts
{
    public class PostService : IPostService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker

        public ValueTask<Post> AddPostAsync(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
