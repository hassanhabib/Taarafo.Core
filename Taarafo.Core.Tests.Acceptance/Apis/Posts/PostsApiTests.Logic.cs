// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Taarafo.Core.Tests.Acceptance.Models.Posts;
using Xunit;

namespace Taarafo.Core.Tests.Acceptance.Apis.Posts
{
    public partial class PostsApiTests
    {
        [Fact]
        public async Task ShouldPostPostAsync()
        {
            // given
            Post randomPost = CreateRandomPost();
            Post inputPost = randomPost;
            Post expectedPost = inputPost;

            // when 
            await this.apiBroker.PostPostAsync(inputPost);

            Post actualPost =
                await this.apiBroker.GetPostByIdAsync(inputPost.Id);

            // then
            actualPost.Should().BeEquivalentTo(expectedPost);
            await this.apiBroker.DeletePostByIdAsync(actualPost.Id);
        }
    }
}
