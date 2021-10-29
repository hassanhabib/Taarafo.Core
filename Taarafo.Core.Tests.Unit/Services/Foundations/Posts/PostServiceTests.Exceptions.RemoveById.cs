// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Posts
{
    public partial class PostServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid somePostId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedPostServiceException =
                    new FailedPostServiceException(serviceException);

            var expectedPostServiceException =
                new PostServiceException(failedPostServiceException);

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectPostByIdAsync(It.IsAny<Guid>()))
                        .ThrowsAsync(serviceException);
            // when
            ValueTask<Post> deletePostTask =
                this.postService.RemovePostByIdAsync(somePostId);

            // then
            await Assert.ThrowsAsync<PostServiceException>(() =>
                deletePostTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {

            // given
            Guid somePostId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedPostStorageException =
                new FailedPostStorageException(sqlException);

            var expectedPostDependencyException =
                new PostDependencyException(failedPostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);
            // when
            ValueTask<Post> deletePostTask =
                this.postService.RemovePostByIdAsync(somePostId);

            // then
            await Assert.ThrowsAsync<PostDependencyException>(() =>
                deletePostTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionIfPostIsNotFoundAndLogItAsync()
        {
            // given
            Guid randomPostId = Guid.NewGuid();
            Guid inputPostId = randomPostId;
            Post noPost = null;

            var notFoundPostException =
                new NotFoundPostException(inputPostId);

            var expectedPostValidationException =
                new PostValidationException(
                    notFoundPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noPost);

            // when
            ValueTask<Post> removePostByIdTask =
                this.postService.RemovePostByIdAsync(inputPostId);

            // then
            await Assert.ThrowsAsync<PostValidationException>(() =>
               removePostByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedPostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePostAsync(It.IsAny<Post>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}