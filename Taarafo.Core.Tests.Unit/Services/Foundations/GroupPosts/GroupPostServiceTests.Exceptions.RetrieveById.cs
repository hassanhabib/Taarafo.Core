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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someGroupPostId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedGroupPostStorageException =
                new FailedGroupPostStorageException(sqlException);

            var expectedGroupPostDependencyException =
                new GroupPostDependencyException(failedGroupPostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<GroupPost> retrieveGroupPostByIdTask =
                this.groupPostService.RetrieveGroupPostByIdAsync(someGroupPostId);

            GroupPostDependencyException actualGroupPostDependencyException =
                await Assert.ThrowsAsync<GroupPostDependencyException>(
                    retrieveGroupPostByIdTask.AsTask);

            //then
            actualGroupPostDependencyException.Should().BeEquivalentTo(
                   expectedGroupPostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(It.IsAny<Guid>()),
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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someGroupPostId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedGroupPostServiceException =
                new FailedGroupPostServiceException(serviceException);

            var expectedGroupPostServiceException =
                new GroupPostServiceException(failedGroupPostServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            //when
            ValueTask<GroupPost> retrieveGroupPostByIdTask =
                this.groupPostService.RetrieveGroupPostByIdAsync(
                    someGroupPostId);

            GroupPostServiceException actualGroupPostServiceException =
                 await Assert.ThrowsAsync<GroupPostServiceException>(
                     retrieveGroupPostByIdTask.AsTask);

            //then
            actualGroupPostServiceException.Should().BeEquivalentTo(
                expectedGroupPostServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(It.IsAny<Guid>()),
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