// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.Groups.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Groups
{
    public partial class GroupServiceTests
    {
        [Fact]
        private void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedGroupStorageException =
                new FailedGroupStorageException(
                    message: "Failed group storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedGroupDependencyException =
                new GroupDependencyException(
                    message: "Group dependency error occurred, contact support.",
                    innerException: failedGroupStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGroups())
                    .Throws(sqlException);

            // when
            Action retrieveAllGroupsAction = () =>
                this.groupService.RetrieveAllGroups();

            GroupDependencyException actualGroupDependencyException =
                Assert.Throws<GroupDependencyException>(retrieveAllGroupsAction);

            // then
            actualGroupDependencyException.Should().BeEquivalentTo(
                expectedGroupDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGroups(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private void ShouldThrowServiceExceptionOnRetrieveAllWhenServiceErrorOccursAndLogIt()
        {
            //given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedGroupServiceException =
                new FailedGroupServiceException(
                    message: "Failed group service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedGroupServiceException =
                new GroupServiceException(
                    message: "Group service error occurred, contact support.",
                    innerException: failedGroupServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGroups())
                    .Throws(serviceException);

            //when
            Action retrieveAllGroupsAction = () =>
                 this.groupService.RetrieveAllGroups();

            GroupServiceException actualGroupServiceException =
                Assert.Throws<GroupServiceException>(retrieveAllGroupsAction);

            //then
            actualGroupServiceException.Should().BeEquivalentTo(
                expectedGroupServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGroups(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}