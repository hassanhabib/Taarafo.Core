﻿// ---------------------------------------------------------------
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
        private async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someCommentId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedCommentException =
                new LockedCommentException(
                    message: "Locked comment record exception, please try again later",
                        innerException: databaseUpdateConcurrencyException);

            var expectedCommentDependencyValidationException =
                new CommentDependencyValidationException(
                    message: "Comment dependency validation occurred, please try again.",
                        innerException: lockedCommentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Comment> removeCommentByIdTask =
                this.commentService.RemoveCommentByIdAsync(someCommentId);

            CommentDependencyValidationException actualCommentDependencyValidationException =
                await Assert.ThrowsAsync<CommentDependencyValidationException>(
                    removeCommentByIdTask.AsTask);

            // then
            actualCommentDependencyValidationException.Should().BeEquivalentTo(
                expectedCommentDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCommentAsync(It.IsAny<Comment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCommentId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedCommentStorageException =
                new FailedCommentStorageException(
                    message: "Failed comment storage error occurred, contact support.",
                        innerException: sqlException);

            var expectedCommentDependencyException =
                new CommentDependencyException(
                    message: "Comment dependency error occurred, contact support.",
                        innerException: failedCommentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Comment> deleteCommentTask =
                this.commentService.RemoveCommentByIdAsync(someCommentId);

            CommentDependencyException actualCommentDependencyException =
                await Assert.ThrowsAsync<CommentDependencyException>(
                    deleteCommentTask.AsTask);

            // then
            actualCommentDependencyException.Should().BeEquivalentTo(
                expectedCommentDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCommentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCommentId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedCommentServiceException =
                new FailedCommentServiceException(
                    message: "Failed comment service occurred, please contact support",
                        innerException: serviceException);

            var expectedCommentServiceException =
                new CommentServiceException(
                    message: "Comment service error occurred, contact support.",
                        innerException: failedCommentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Comment> removeCommentByIdTask =
                this.commentService.RemoveCommentByIdAsync(someCommentId);

            CommentServiceException actualCommentServiceException =
                await Assert.ThrowsAsync<CommentServiceException>(
                    removeCommentByIdTask.AsTask);

            // then
            actualCommentServiceException.Should().BeEquivalentTo(expectedCommentServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}