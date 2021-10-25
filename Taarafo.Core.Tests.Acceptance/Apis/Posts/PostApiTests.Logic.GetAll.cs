// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Taarafo.Core.Tests.Acceptance.Models.Posts;
using Xunit;

namespace Taarafo.Core.Tests.Acceptance.Apis.Posts
{
    public partial class PostApiTests
    {
        [Fact]
        public async Task ShouldGetAllPostsAsync()
        {
            // given
            IEnumerable<Post> randomPosts = CreateRandomPosts();
            IEnumerable<Post> inputPosts = randomPosts;

            foreach (Post post in inputPosts)
            {
                await this.apiBroker.PostPostAsync(post);
            }

            List<Post> expectedPosts = inputPosts.ToList();

            // when
            List<Post> actualPosts = await this.apiBroker.GetAllPostsAsync();

            // then
            foreach (Post expectedPost in expectedPosts)
            {
                Post actualPost = actualPosts.Single(post => post.Id == expectedPost.Id);
                actualPost.Should().BeEquivalentTo(expectedPost);
                await this.apiBroker.DeletePostByIdAsync(actualPost.Id);
            }
        }
    }
}
