using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Taarafo.Core.Tests.Acceptance.Models.Posts;
using Xunit;

namespace Taarafo.Core.Tests.Acceptance.Apis.Posts
{
    public partial class PostsApiTests
    {
        [Fact]
        public async Task ShouldGetAllPostsAsync()
        {
            // given
            List<Post> randomPosts = await CreateRandomPostedPostsAsync();

            List<Post> expectedPosts = randomPosts;

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
