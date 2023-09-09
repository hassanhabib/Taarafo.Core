// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.GroupPosts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupPosts
{
    public partial class GroupPostServiceTests
    {
        [Fact]
        private void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            //given
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
                broker.SelectAllGroupPosts()).Throws(sqlException);

            //when
            Action retrieveAllGroupPostsAction = () =>
                this.groupPostService.RetrieveAllGroupPosts();

            GroupPostDependencyException actualGroupPostDependencyException =
                Assert.Throws<GroupPostDependencyException>(retrieveAllGroupPostsAction);

            //then
            actualGroupPostDependencyException.Should().BeEquivalentTo(
                expectedGroupPostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGroupPosts(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            //given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedGroupPostServiceException =
                new FailedGroupPostServiceException(
                    message: "Failed group post service occurred, please contact support.",
                    innerException: serviceException);

            var expectedGroupPostServiceException =
                new GroupPostServiceException(
                    message: "Group post service error occurred, please contact support.",
                    innerException: failedGroupPostServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGroupPosts()).Throws(serviceException);

            //when
            Action retrieveAllGroupPostAction = () =>
                this.groupPostService.RetrieveAllGroupPosts();

            GroupPostServiceException actualGroupPostServiceException =
                Assert.Throws<GroupPostServiceException>(retrieveAllGroupPostAction);

            //then
            actualGroupPostServiceException.Should().BeEquivalentTo(expectedGroupPostServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGroupPosts(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}