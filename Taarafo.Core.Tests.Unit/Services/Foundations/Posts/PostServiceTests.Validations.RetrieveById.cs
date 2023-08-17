// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Posts
{
    public partial class PostServiceTests
    {
        [Fact]
        private async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidPostId = Guid.Empty;

            var invalidPostException =
                new InvalidPostException();

            invalidPostException.AddData(
                key: nameof(Post.Id),
                values: "Id is required");

            var expectedPostValidationException = new
                PostValidationException(
                message: "Post validation errors occurred, please try again.",
                    innerException: invalidPostException);

            // when
            ValueTask<Post> retrievePostByIdTask =
                this.postService.RetrievePostByIdAsync(invalidPostId);

            PostValidationException actualPostValidationException =
                await Assert.ThrowsAsync<PostValidationException>(
                    retrievePostByIdTask.AsTask);

            // then
            actualPostValidationException.Should().BeEquivalentTo(expectedPostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfPostIsNotFoundAndLogItAsync()
        {
            //given
            Guid somePostId = Guid.NewGuid();
            Post noPost = null;
            var nullPostException = new Exception();

            var notFoundPostException =
                new NotFoundPostException(
                    message: $"Couldn't find post with id: {somePostId}.",
                        innerException: nullPostException);

            var expectedPostValidationException =
                new PostValidationException(
                    message: "Post validation errors occurred, please try again.",
                        innerException: notFoundPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noPost);

            //when
            ValueTask<Post> retrievePostByIdTask =
                this.postService.RetrievePostByIdAsync(somePostId);

            PostValidationException actualPostValidationException =
                await Assert.ThrowsAsync<PostValidationException>(
                    retrievePostByIdTask.AsTask);

            // then
            actualPostValidationException.Should().BeEquivalentTo(expectedPostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}