// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.Posts;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Posts
{
    public partial class PostServiceTests
    {
        [Fact]
        public async Task ShouldAddPostAsync()
        {
            // given
            Post randomPost = CreateRandomPost();
            Post inputPost = randomPost;
            Post storagePost = inputPost;
            Post expectedPost = storagePost;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPostAsync(inputPost))
                    .ReturnsAsync(storagePost);

            // when
            Post actualPost = await this.postService
                .AddPostAsync(inputPost);

            // then
            actualPost.Should().BeEquivalentTo(expectedPost);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostAsync(inputPost),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrievePostByIdAsync()
        {
            //given
            Guid randomPostId = Guid.NewGuid();
            Guid inputPostId = randomPostId;
            Post randomPost = CreateRandomPost();
            Post storagePost = randomPost;
            Post expectedPost = storagePost;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(inputPostId))
                    .ReturnsAsync(storagePost);

            //when
            Post actualPost =
                await this.postService.RetrievePostByIdAsync(inputPostId);

            //then
            actualPost.Should().BeEquivalentTo(expectedPost);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(inputPostId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllPosts()
        {
            //given
            IQueryable<Post> randomPosts = CreateRandomPosts();
            IQueryable<Post> storagePosts = randomPosts;
            IQueryable<Post> expectedPosts = storagePosts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPosts())
                    .Returns(storagePosts);

            //when
            IQueryable<Post> actualPosts = this.postService.RetrieveAllPosts();

            //then
            actualPosts.Should().BeEquivalentTo(expectedPosts);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPosts(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyPostAsync()
        {
            //given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            Post randomPost = CreateRandomPost();
            Post inputPost = randomPost;
            Post afterUpdateStoragePost = inputPost;
            Post expectedPost = afterUpdateStoragePost;
            Post beforeUpdateStoragePost = randomPost.DeepClone();
            inputPost.UpdatedDate = randomDate;
            Guid postId = inputPost.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(postId))
                    .ReturnsAsync(beforeUpdateStoragePost);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdatePostAsync(inputPost))
                    .ReturnsAsync(afterUpdateStoragePost);

            //when
            Post originalPost = await this.postService.RetrievePostByIdAsync(postId);
            Post modifiedPost = await this.postService.ModifyPostAsync(inputPost);

            //then
            originalPost.Should().BeEquivalentTo(beforeUpdateStoragePost);
            modifiedPost.Should().BeEquivalentTo(expectedPost);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(postId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePostAsync(inputPost),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRemovePostByIdAsync()
        {
            //given
            Post randomPost = CreateRandomPost();
            Post storagePost = randomPost;
            Post expectedPost = storagePost;
            Guid postId = randomPost.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(postId))
                    .ReturnsAsync(storagePost);

            this.storageBrokerMock.Setup(broker =>
                broker.DeletePostAsync(storagePost))
                    .ReturnsAsync(expectedPost);

            //when
            Post actualPost = await this.postService.RemovePostByIdAsync(postId);

            //then
            actualPost.Should().BeEquivalentTo(expectedPost);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(postId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePostAsync(storagePost),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
