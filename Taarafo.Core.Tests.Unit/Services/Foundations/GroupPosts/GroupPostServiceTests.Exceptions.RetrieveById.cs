// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.GroupPosts;
using Taarafo.Core.Models.GroupPosts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupPosts
{
    public partial class GroupPostServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someGroupId = Guid.NewGuid();
            Guid somePostId = Guid.NewGuid();
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
                broker.SelectGroupPostByIdAsync(someGroupId, somePostId))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<GroupPost> retrieveGroupPostByIdTask =
                this.groupPostService.RetrieveGroupPostByIdAsync(someGroupId, somePostId);

            GroupPostDependencyException actualGroupPostDependencyException =
                await Assert.ThrowsAsync<GroupPostDependencyException>(
                    retrieveGroupPostByIdTask.AsTask);

            //then
            actualGroupPostDependencyException.Should().BeEquivalentTo(
                   expectedGroupPostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(It.IsAny<Guid>(), (It.IsAny<Guid>())),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someGroupId = Guid.NewGuid();
            Guid somePostId = Guid.NewGuid();
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
                broker.SelectGroupPostByIdAsync(someGroupId, somePostId))
                    .ThrowsAsync(serviceException);

            //when
            ValueTask<GroupPost> retrieveGroupPostByIdTask =
                this.groupPostService.RetrieveGroupPostByIdAsync(
                    someGroupId, somePostId);

            GroupPostServiceException actualGroupPostServiceException =
                 await Assert.ThrowsAsync<GroupPostServiceException>(
                     retrieveGroupPostByIdTask.AsTask);

            //then
            actualGroupPostServiceException.Should().BeEquivalentTo(
                expectedGroupPostServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(
                    It.IsAny<Guid>(), (It.IsAny<Guid>())),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}