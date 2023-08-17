// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.Posts;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Posts
{
    private partial class PostServiceTests
    {
        [Fact]
        public async Task ShouldRemovePostByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputPostId = randomId;
            Post randomPost = CreateRandomPost();
            Post storagePost = randomPost;
            Post expectedInputPost = storagePost;
            Post deletedPost = expectedInputPost;
            Post expectedPost = deletedPost.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(inputPostId))
                    .ReturnsAsync(storagePost);

            this.storageBrokerMock.Setup(broker =>
                broker.DeletePostAsync(expectedInputPost))
                    .ReturnsAsync(deletedPost);

            // when
            Post actualPost = await this.postService
                .RemovePostByIdAsync(inputPostId);

            // then
            actualPost.Should().BeEquivalentTo(expectedPost);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(inputPostId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePostAsync(expectedInputPost),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}