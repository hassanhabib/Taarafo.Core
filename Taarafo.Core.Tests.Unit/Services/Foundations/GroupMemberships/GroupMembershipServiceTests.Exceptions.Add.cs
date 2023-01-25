// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            GroupMembership someGroupMembership = CreateRandomGroupMembership(randomDateTime);
            SqlException sqlException = GetSqlException();

            var failedGroupMembershipStorageException =
                new FailedGroupMembershipStorageException(sqlException);

            var expectedGroupMembershipDependencyException =
                new GroupMembershipDependencyException(failedGroupMembershipStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            //when
            ValueTask<GroupMembership> addGroupMembershipTask =
                this.groupMembershipService.AddGroupMembershipAsync(someGroupMembership);

            GroupMembershipDependencyException actualGroupMembershipDependencyException =
                await Assert.ThrowsAsync<GroupMembershipDependencyException>(
                    addGroupMembershipTask.AsTask);

            //then
            actualGroupMembershipDependencyException.Should().BeEquivalentTo(
                expectedGroupMembershipDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupMembershipDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupMembershipAsync(It.IsAny<GroupMembership>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfGroupMembershipAlreadyExistsAndLogItAsync()
        {
            //given
            GroupMembership randomGroupMembership = CreateRandomGroupMembership();
            GroupMembership alreadyExistsGroupMembership = randomGroupMembership;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsGroupMembershipException =
                new AlreadyExistsGroupMembershipException(duplicateKeyException);

            var expectedGroupMembershipDependencyValidationException =
                new GroupMembershipDependencyValidationException(alreadyExistsGroupMembershipException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            //when
            ValueTask<GroupMembership> addGroupMembershipTask =
                this.groupMembershipService.AddGroupMembershipAsync(alreadyExistsGroupMembership);

            GroupMembershipDependencyValidationException actualGroupMembershipDependencyValidationException =
                await Assert.ThrowsAsync<GroupMembershipDependencyValidationException>(
                    addGroupMembershipTask.AsTask);

            //then
            actualGroupMembershipDependencyValidationException.Should().BeEquivalentTo(
                expectedGroupMembershipDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupMembershipAsync(It.IsAny<GroupMembership>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupMembershipDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}