// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Taarafo.Core.Tests.Acceptance.Models.Posts;

namespace Taarafo.Core.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        public ValueTask<Post> PostPostAsync(Post post) =>
            throw new NotImplementedException();

        public async ValueTask<Post> GetPostByIdAsync(Guid postId) =>
            throw new NotImplementedException();

        public async ValueTask<Post> DeletePostByIdAsync(Guid postId) =>
            throw new NotImplementedException();
    }
}
