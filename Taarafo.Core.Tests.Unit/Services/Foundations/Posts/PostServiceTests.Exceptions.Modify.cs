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
        private async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTimeOffset();
            Post randomPost = CreateRandomPost(someDateTime);
            Post somePost = randomPost;
            Guid postId = somePost.Id;
            SqlException sqlException = GetSqlException();

            var failedPostStorageException =
                new FailedPostStorageException(
                    message: "Failed post storage error occurred, contact support.",
                        innerException: sqlException);

            var expectedPostDependencyException =
                new PostDependencyException(
                    message: "Post dependency error occurred, contact support.",
                    innerException: failedPostStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Post> modifyPostTask =
                this.postService.ModifyPostAsync(somePost);

            PostDependencyException actualPostDependencyException =
              await Assert.ThrowsAsync<PostDependencyException>(
                  modifyPostTask.AsTask);

            // then
            actualPostDependencyException.Should().BeEquivalentTo(
                expectedPostDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(postId),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePostAsync(somePost),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Post randomPost = CreateRandomPost(randomDateTime);
            Post SomePost = randomPost;
            Guid postId = SomePost.Id;

            SomePost.CreatedDate =
                randomDateTime.AddMinutes(minutesInPast);

            var databaseUpdateException =
                new DbUpdateException();

            var failedPostException =
                new FailedPostStorageException(
                    message: "Failed post storage error occurred, contact support.",
                        innerException: databaseUpdateException);

            var expectedPostDependencyException =
                new PostDependencyException(
                    message: "Post dependency error occurred, contact support.",
                        innerException: failedPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(postId))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Post> modifyPostTask =
                this.postService.ModifyPostAsync(SomePost);

            PostDependencyException actualPostDependencyException =
              await Assert.ThrowsAsync<PostDependencyException>(
                  modifyPostTask.AsTask);

            // then
            actualPostDependencyException.Should().BeEquivalentTo(
                expectedPostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(postId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Post randomPost = CreateRandomPost(randomDateTime);
            Post somePost = randomPost;
            somePost.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            Guid postId = somePost.Id;

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
                broker.SelectPostByIdAsync(postId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Post> modifyPostTask =
                this.postService.ModifyPostAsync(somePost);

            PostDependencyValidationException actualPostDependencyValidationException =
                await Assert.ThrowsAsync<PostDependencyValidationException>(
                    modifyPostTask.AsTask);

            // then
            actualPostDependencyValidationException.Should().BeEquivalentTo(
                expectedPostDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(postId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            int minuteInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Post randomPost = CreateRandomPost(randomDateTime);
            Post somePost = randomPost;
            somePost.CreatedDate = randomDateTime.AddMinutes(minuteInPast);
            var serviceException = new Exception();

            var failedPostException =
                new FailedPostServiceException(
                    message: "Failed post service occurred, please contact support",
                        innerException: serviceException);

            var expectedPostServiceException =
                new PostServiceException(
                    message: "Post service error occurred, contact support.",
                        innerException: failedPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(somePost.Id))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Post> modifyPostTask =
                this.postService.ModifyPostAsync(somePost);

            PostServiceException actualPostServiceException =
                await Assert.ThrowsAsync<PostServiceException>(
                    modifyPostTask.AsTask);

            // then
            actualPostServiceException.Should().BeEquivalentTo(
                expectedPostServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(somePost.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}