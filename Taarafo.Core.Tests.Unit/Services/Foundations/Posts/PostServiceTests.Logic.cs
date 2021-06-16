// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
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
    }
}
