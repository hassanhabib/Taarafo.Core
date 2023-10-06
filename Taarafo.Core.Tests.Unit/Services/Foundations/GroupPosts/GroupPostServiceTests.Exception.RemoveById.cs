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
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid groupId = Guid.NewGuid();
            Guid postId = Guid.NewGuid();

            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedGroupPostException =
                new LockedGroupPostException(databaseUpdateConcurrencyException);

            var expectedGroupPostDependencyValidationException =
                new GroupPostDependencyValidationException(lockedGroupPostException);

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

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid groupId = Guid.NewGuid();
            Guid postId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedGroupPostStorageException =
                new FailedGroupPostStorageException(sqlException);

            var expectedGroupPostDependencyException =
                new GroupPostDependencyException(failedGroupPostStorageException);

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
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid groupId = Guid.NewGuid();
            Guid postId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedGroupPostServiceException =
                new FailedGroupPostServiceException(serviceException);

            var expectedGroupPostServiceException =
                new GroupPostServiceException(failedGroupPostServiceException);

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
