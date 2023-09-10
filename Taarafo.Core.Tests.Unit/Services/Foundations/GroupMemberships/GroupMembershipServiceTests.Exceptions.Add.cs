// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Taarafo.Core.Models.GroupMemberships;
using Taarafo.Core.Models.GroupMemberships.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupMemberships
{
    public partial class GroupMembershipServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            GroupMembership someGroupMembership = CreateRandomGroupMembership(randomDateTime);
            SqlException sqlException = GetSqlException();

            var failedGroupMembershipStorageException =
                new FailedGroupMembershipStorageException(
                    message: "Failed GroupMembership storage error occured, contact support.",
                    innerException: sqlException);

            var expectedGroupMembershipDependencyException =
                new GroupMembershipDependencyException(
                    message: "GroupMembership dependency validation occurred, please try again.",
                    innerException: failedGroupMembershipStorageException);

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
        private async Task ShouldThrowDependencyValidationExceptionOnAddIfGroupMembershipAlreadyExistsAndLogItAsync()
        {
            //given
            GroupMembership randomGroupMembership = CreateRandomGroupMembership();
            GroupMembership alreadyExistsGroupMembership = randomGroupMembership;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsGroupMembershipException =
                new AlreadyExistsGroupMembershipException(
                    message: "GroupMembership with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedGroupMembershipDependencyValidationException =
                new GroupMembershipDependencyValidationException(
                    message: "GroupMembership dependency validation occurred, please try again.",
                    innerException: alreadyExistsGroupMembershipException);

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

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupMembershipDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupMembershipAsync(It.IsAny<GroupMembership>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            //given
            GroupMembership someGroupMembership = CreateRandomGroupMembership();

            var databaseUpdateException = new DbUpdateException();

            var failedGroupMembershipStorageException =
                new FailedGroupMembershipStorageException(
                    message: "Failed GroupMembership storage error occured, contact support.",
                    innerException: databaseUpdateException);

            var expectedGroupMembershipDependencyException =
                new GroupMembershipDependencyException(
                    message: "GroupMembership dependency validation occurred, please try again.",
                    innerException: failedGroupMembershipStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupMembershipDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupMembershipAsync(It.IsAny<GroupMembership>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            GroupMembership someGroupMembership = CreateRandomGroupMembership();
            var serviceException = new Exception();

            var failedGroupMembershipServiceException =
                new FailedGroupMembershipServiceException(
                    message: "Failed GroupMembership service occurred, please contact support.",
                    innerException: serviceException);

            var expectedGroupMembershipServiceException =
                new GroupMembershipServiceException(
                    message: "GroupMembership service error occurred, please contact support.",
                    innerException: failedGroupMembershipServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<GroupMembership> addGroupMembershipTask =
                this.groupMembershipService.AddGroupMembershipAsync(
                    someGroupMembership);

            GroupMembershipServiceException actualGroupMembershipServiceException =
                await Assert.ThrowsAsync<GroupMembershipServiceException>(
                    addGroupMembershipTask.AsTask);

            // then
            actualGroupMembershipServiceException.Should().BeEquivalentTo(
                expectedGroupMembershipServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupMembershipServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupMembershipAsync(It.IsAny<GroupMembership>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}