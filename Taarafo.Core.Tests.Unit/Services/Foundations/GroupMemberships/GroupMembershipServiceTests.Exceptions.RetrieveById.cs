// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.GroupMemberships;
using Taarafo.Core.Models.GroupMemberships.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupMemberships
{
    public partial class GroupMembershipServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedGroupMembershipStorageException =
                new FailedGroupMembershipStorageException(sqlException);

            var expectedGroupMembershipDependencyException =
                new GroupMembershipDependencyException(failedGroupMembershipStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupMembershipByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<GroupMembership> retrieveGroupMembershipByIdTask =
                this.groupMembershipService.RetrieveGroupMembershipByIdAsync(someId);

            GroupMembershipDependencyException actualGroupMembershipDependencyException =
                await Assert.ThrowsAsync<GroupMembershipDependencyException>(
                    retrieveGroupMembershipByIdTask.AsTask);

            // then
            actualGroupMembershipDependencyException.Should().BeEquivalentTo(
                expectedGroupMembershipDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupMembershipByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupMembershipDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedGroupMembershipServiceException =
                new FailedGroupMembershipServiceException(serviceException);

            var expectedGroupMembershipServiceException =
                new GroupMembershipServiceException(failedGroupMembershipServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupMembershipByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<GroupMembership> retrieveGroupMembershipByIdTask =
                this.groupMembershipService.RetrieveGroupMembershipByIdAsync(someId);

            GroupMembershipServiceException actualGroupMembershipServiceException =
                await Assert.ThrowsAsync<GroupMembershipServiceException>(
                    retrieveGroupMembershipByIdTask.AsTask);

            // then
            actualGroupMembershipServiceException.Should().BeEquivalentTo(
                expectedGroupMembershipServiceException);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectGroupMembershipByIdAsync(It.IsAny<Guid>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedGroupMembershipServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}