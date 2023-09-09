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
        private async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTimeOffset();
            GroupPost randomGroupPost = CreateRandomGroupPost(someDateTime);
            GroupPost someGroupPost = randomGroupPost;
            Guid groupId = someGroupPost.GroupId;
            Guid postId = someGroupPost.PostId;
            SqlException sqlException = CreateSqlException();

            var failedGroupPostStorageException =
                new FailedGroupPostStorageException(
                    message: "Failed group post storage error occured, contact support.",
                    innerException: sqlException);

            var expectedGroupPostDependencyException =
                new GroupPostDependencyException(
                    message: "Group post dependency validation occurred, please try again.",
                    innerException: failedGroupPostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(groupId, postId))
                    .Throws(sqlException);

            // when
            ValueTask<GroupPost> modifyGroupPostTask =
                this.groupPostService.ModifyGroupPostAsync(someGroupPost);

            GroupPostDependencyException actualGroupPostDependencyException =
                await Assert.ThrowsAsync<GroupPostDependencyException>(
                     modifyGroupPostTask.AsTask);

            // then
            actualGroupPostDependencyException.Should().BeEquivalentTo(
                expectedGroupPostDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(groupId, postId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGroupPostAsync(someGroupPost), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            GroupPost randomGroupPost = CreateRandomGroupPost(randomDateTime);
            GroupPost someGroupPost = randomGroupPost;
            Guid groupId = someGroupPost.GroupId;
            Guid postId = someGroupPost.PostId;
            var databaseUpdateException = new DbUpdateException();

            var failedGroupPostException =
            new FailedGroupPostStorageException(
                message: "Failed group post storage error occured, contact support.",
                innerException: databaseUpdateException);

            var expectedGroupPostDependencyException =
                new GroupPostDependencyException(
                    message: "Group post dependency validation occurred, please try again.",
                    innerException: failedGroupPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(groupId, postId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<GroupPost> modifyGroupPostTask =
                this.groupPostService.ModifyGroupPostAsync(someGroupPost);

            GroupPostDependencyException actualGroupPostDependencyException =
                 await Assert.ThrowsAsync<GroupPostDependencyException>(
                     modifyGroupPostTask.AsTask);

            // then
            actualGroupPostDependencyException.Should().BeEquivalentTo(
                expectedGroupPostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(groupId, postId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            GroupPost randomGroupPost = CreateRandomGroupPost(randomDateTime);
            GroupPost someGroupPost = randomGroupPost;
            Guid groupId = someGroupPost.GroupId;
            Guid postId = someGroupPost.PostId;
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
                broker.SelectGroupPostByIdAsync(groupId, postId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<GroupPost> modifyGroupPostTask =
            this.groupPostService.ModifyGroupPostAsync(someGroupPost);
            GroupPostDependencyValidationException actualGroupPostDependencyValidationException =
                await Assert.ThrowsAsync<GroupPostDependencyValidationException>(
                    modifyGroupPostTask.AsTask);

            // then
            actualGroupPostDependencyValidationException.Should().BeEquivalentTo(
                expectedGroupPostDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(groupId, postId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            int minuteInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            GroupPost randomGroupPost = CreateRandomGroupPost(randomDateTime);
            GroupPost someGroupPost = randomGroupPost;
            var serviceException = new Exception();

            var failedGroupPostException =
                new FailedGroupPostServiceException(
                    message: "Failed group post service occurred, please contact support.",
                    innerException: serviceException);

            var expectedGroupPostServiceException =
                new GroupPostServiceException(
                     message: "Group post service error occurred, please contact support.",
                    innerException: failedGroupPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(someGroupPost.GroupId, someGroupPost.PostId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<GroupPost> modifyGroupPostTask =
                this.groupPostService.ModifyGroupPostAsync(someGroupPost);

            GroupPostServiceException actualGroupPostServiceException =
                await Assert.ThrowsAsync<GroupPostServiceException>(
                    modifyGroupPostTask.AsTask);

            // then
            actualGroupPostServiceException.Should().BeEquivalentTo(
                expectedGroupPostServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(someGroupPost.GroupId, someGroupPost.PostId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}