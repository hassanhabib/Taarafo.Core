// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using RESTFulSense.Exceptions;
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

        [Fact]
        public async Task ShouldGetAllPostsAsync()
        {
            // given
            List<Post> randomPosts = await CreateRandomPostedPostsAsync();

            List<Post> expectedPosts = randomPosts;

            // when
            List<Post> actualPosts = await this.apiBroker.GetAllPostsAsync();

            // then
            actualPosts.Count.Should().BeGreaterThanOrEqualTo(expectedPosts.Count);
            actualPosts.Count.Should().BeLessThanOrEqualTo(10);

            foreach (Post expectedPost in expectedPosts)
            {
                if (actualPosts.Any(post => post.Id == expectedPost.Id))
                {
                    Post actualPost = actualPosts.Single(post => post.Id == expectedPost.Id);
                    actualPost.Should().BeEquivalentTo(expectedPost);
                    await this.apiBroker.DeletePostByIdAsync(actualPost.Id);
                }
            }
        }

        [Fact]
        public async Task ShouldDeletePostAsync()
        {
            // given
            Post randomPost = await PostRandomPostAsync();
            Post inputPost = randomPost;
            Post expectedPost = inputPost;

            // when
            Post deletedPost =
                await this.apiBroker.DeletePostByIdAsync(inputPost.Id);

            ValueTask<Post> getPostbyIdTask =
                this.apiBroker.GetPostByIdAsync(inputPost.Id);

            // then
            deletedPost.Should().BeEquivalentTo(expectedPost);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
                getPostbyIdTask.AsTask());
        }
    }
}
