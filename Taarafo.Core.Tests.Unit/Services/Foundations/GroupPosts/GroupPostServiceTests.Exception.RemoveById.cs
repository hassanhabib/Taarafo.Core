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
using Taarafo.Core.Models.GroupPosts;
using Taarafo.Core.Models.GroupPosts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupPosts
{
    public partial class GroupPostServiceTests
    {
        [Fact]
        private async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid groupId = Guid.NewGuid();
            Guid postId = Guid.NewGuid();

            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedGroupPostException =
                new LockedGroupPostException(
                    message: "GroupPost is locked, please try again.",
                    innerException: databaseUpdateConcurrencyException);

            var expectedGroupPostDependencyValidationException =
                new GroupPostDependencyValidationException(
                     message: "Group post dependency validation occurred, please try again.",
                    innerException: lockedGroupPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<GroupPost> removeGroupPostByIdTask =
                this.groupPostService.RemoveGroupPostByIdAsync(groupId, postId);

            GroupPostDependencyValidationException actualGroupPostDependencyValidationException =
                await Assert.ThrowsAsync<GroupPostDependencyValidationException>(
                    removeGroupPostByIdTask.AsTask);

            // then
            actualGroupPostDependencyValidationException.Should().BeEquivalentTo(
                expectedGroupPostDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGroupPostAsync(It.IsAny<GroupPost>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid groupId = Guid.NewGuid();
            Guid postId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedGroupPostStorageException =
                new FailedGroupPostStorageException(
                    message: "Failed group post storage error occured, contact support.",
                    innerException: sqlException);

            var expectedGroupPostDependencyException =
                new GroupPostDependencyException(
                    message: "Group post dependency validation occurred, please try again.",
                    innerException: failedGroupPostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<GroupPost> deleteGroupPostTask =
                this.groupPostService.RemoveGroupPostByIdAsync(groupId, postId);

            GroupPostDependencyException actualGroupPostDependencyException =
                await Assert.ThrowsAsync<GroupPostDependencyException>(
                    deleteGroupPostTask.AsTask);

            // then
            actualGroupPostDependencyException.Should().BeEquivalentTo(
                expectedGroupPostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid groupId = Guid.NewGuid();
            Guid postId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedGroupPostServiceException =
                new FailedGroupPostServiceException(
                    message: "Failed group post service occurred, please contact support.",
                    innerException: serviceException);

            var expectedGroupPostServiceException =
                new GroupPostServiceException(
                     message: "Group post service error occurred, please contact support.",
                    innerException: failedGroupPostServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<GroupPost> removeGroupPostByIdTask =
                this.groupPostService.RemoveGroupPostByIdAsync(groupId, postId);

            GroupPostServiceException actualGroupPostServiceException =
                await Assert.ThrowsAsync<GroupPostServiceException>(
                    removeGroupPostByIdTask.AsTask);

            // then
            actualGroupPostServiceException.Should().BeEquivalentTo(
                expectedGroupPostServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}