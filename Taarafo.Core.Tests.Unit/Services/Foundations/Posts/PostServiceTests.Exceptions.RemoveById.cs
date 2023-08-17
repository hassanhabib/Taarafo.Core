// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Posts
{
    public partial class PostServiceTests
    {
        [Fact]
        private async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid somePostId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedPostException =
                new LockedPostException(
                    message: "Locked post record exception, please try again later",
                        innerException: databaseUpdateConcurrencyException);

            var expectedPostDependencyValidationException =
                new PostDependencyValidationException(
                    message: "Post dependency validation occurred, please try again.",
                        innerException: lockedPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Post> removePostByIdTask =
                this.postService.RemovePostByIdAsync(somePostId);

            PostDependencyValidationException actualPostDependencyValidationException =
                await Assert.ThrowsAsync<PostDependencyValidationException>(
                    removePostByIdTask.AsTask);

            // then
            actualPostDependencyValidationException.Should().BeEquivalentTo(
                expectedPostDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePostAsync(It.IsAny<Post>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid somePostId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedPostStorageException =
                new FailedPostStorageException(
                    message: "Failed post storage error occurred, contact support.",
                        innerException: sqlException);

            var expectedPostDependencyException =
                new PostDependencyException(
                    message: "Post dependency error occurred, contact support.",
                        innerException: failedPostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);
            // when
            ValueTask<Post> deletePostTask =
                this.postService.RemovePostByIdAsync(somePostId);

            PostDependencyException actualPostDependencyException =
                await Assert.ThrowsAsync<PostDependencyException>(
                    deletePostTask.AsTask);

            // then
            actualPostDependencyException.Should().BeEquivalentTo(
                expectedPostDependencyException);

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
        private async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid somePostId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedPostServiceException =
                new FailedPostServiceException(
                    message: "Failed post service occurred, please contact support",
                        innerException: serviceException);

            var expectedPostServiceException =
                new PostServiceException(
                    message: "Post service error occurred, contact support.",
                        innerException: failedPostServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Post> removePostByIdTask =
                this.postService.RemovePostByIdAsync(somePostId);

            PostServiceException actualPostServiceException =
                await Assert.ThrowsAsync<PostServiceException>(
                    removePostByIdTask.AsTask);

            // then
            actualPostServiceException.Should().BeEquivalentTo(
                expectedPostServiceException);

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
    }
}