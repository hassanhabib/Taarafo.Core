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
using Taarafo.Core.Models.Comments;
using Taarafo.Core.Models.Comments.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Comment randomComment = CreateRandomComment();
            SqlException sqlException = GetSqlException();

            var failedCommentStorageException =
                new FailedCommentStorageException(
                    message: "Failed comment storage error occurred, contact support.",
                        innerException: sqlException);

            var expectedCommentDependencyException =
                new CommentDependencyException(
                    message: "Comment dependency error occurred, contact support.",
                        innerException: failedCommentStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Comment> addCommentTask =
                this.commentService.ModifyCommentAsync(randomComment);

            CommentDependencyException acutalCommentDependencyException =
                await Assert.ThrowsAsync<CommentDependencyException>(
                    addCommentTask.AsTask);

            // then
            acutalCommentDependencyException.Should().BeEquivalentTo(
                expectedCommentDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(randomComment.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCommentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCommentAsync(randomComment),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Comment randomComment = CreateRandomComment();
            var databaseUpdateException = new DbUpdateException();

            var failedCommentException =
                new FailedCommentStorageException(
                    message: "Failed comment storage error occurred, contact support.",
                        innerException: databaseUpdateException);

            var expectedCommentDependencyException =
                new CommentDependencyException(
                    message: "Comment dependency error occurred, contact support.",
                        innerException: failedCommentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            //when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(randomComment);

            CommentDependencyException actualCommentDependencyException =
                await Assert.ThrowsAsync<CommentDependencyException>(
                    modifyCommentTask.AsTask);

            // then
            actualCommentDependencyException.Should().BeEquivalentTo(
                expectedCommentDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(randomComment.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCommentAsync(randomComment),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Comment randomComment = CreateRandomComment();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedCommentException =
                new LockedCommentException(
                    message: "Locked comment record exception, please try again later",
                        innerException: databaseUpdateConcurrencyException);

            var expectedCommentDependencyValidationException =
                new CommentDependencyValidationException(
                    message: "Comment dependency validation occurred, please try again.",
                        innerException: lockedCommentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(randomComment);

            CommentDependencyValidationException actualCommentDependencyValidationException =
                await Assert.ThrowsAsync<CommentDependencyValidationException>(
                    modifyCommentTask.AsTask);

            // then
            actualCommentDependencyValidationException.Should().BeEquivalentTo(
                expectedCommentDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(randomComment.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCommentAsync(randomComment),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Comment randomComment = CreateRandomComment();
            var serviceException = new Exception();

            var failedCommentException =
                new FailedCommentServiceException(
                    message: "Failed comment service occurred, please contact support",
                        innerException: serviceException);

            var expectedCommentServiceException =
                new CommentServiceException(
                    message: "Comment service error occurred, contact support.",
                        innerException: failedCommentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(randomComment);

            CommentServiceException actualCommentServiceException =
                await Assert.ThrowsAsync<CommentServiceException>(
                    modifyCommentTask.AsTask);

            // then
            actualCommentServiceException.Should().BeEquivalentTo(
                expectedCommentServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(randomComment.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCommentAsync(randomComment),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}