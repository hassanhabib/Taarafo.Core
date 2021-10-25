// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taarafo.Core.Tests.Acceptance.Models.Posts;

namespace Taarafo.Core.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string PostRelativeUrl = "api/posts";

        public async ValueTask<Post> PostPostAsync(Post post) =>
               await this.apiFactoryClient.PostContentAsync(
                   PostRelativeUrl, post);

        public async ValueTask<Post> DeletePostByIdAsync(Guid PostId) =>
                await this.apiFactoryClient.DeleteContentAsync<Post>(
                    $"{PostRelativeUrl}/{PostId}");

        public async ValueTask<List<Post>> GetAllPostsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Post>>($"{PostRelativeUrl}/");
    }
}
