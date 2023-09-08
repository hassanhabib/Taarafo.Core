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
using Taarafo.Core.Models.Groups;
using Taarafo.Core.Models.Groups.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Groups
{
    public partial class GroupServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnCreateIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Group someGroup = CreateRandomGroup(randomDateTime);
            SqlException sqlException = GetSqlException();

            var failedGroupStorageException =
                new FailedGroupStorageException(
                    message: "Failed group storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedGroupDependencyException =
                new GroupDependencyException(
                    message: "Group dependency error occurred, contact support.",
                    innerException: failedGroupStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Group> addGroupTask =
                this.groupService.AddGroupAsync(someGroup);

            GroupDependencyException actualGroupDependencyException =
                 await Assert.ThrowsAsync<GroupDependencyException>(
                    addGroupTask.AsTask);

            // then
            actualGroupDependencyException.Should().BeEquivalentTo(
                expectedGroupDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupAsync(It.IsAny<Group>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnCreateIfGroupAlreadyExsitsAndLogItAsync()
        {
            // given
            Group randomGroup = CreateRandomGroup();
            Group alreadyExistsGroup = randomGroup;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsGroupException =
                new AlreadyExistsGroupException(
                    message: "Group with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedGroupDependencyValidationException =
                new GroupDependencyValidationException(
                    message: "Group dependency validation occurred, please try again.",
                    innerException: alreadyExistsGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
              broker.GetCurrentDateTimeOffset())
                  .Throws(duplicateKeyException);

            // when
            ValueTask<Group> addGroupTask =
                this.groupService.AddGroupAsync(alreadyExistsGroup);

            GroupDependencyValidationException actualGroupDependencyValidationException =
                 await Assert.ThrowsAsync<GroupDependencyValidationException>(
                    addGroupTask.AsTask);

            // then
            actualGroupDependencyValidationException.Should().BeEquivalentTo(
                expectedGroupDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupAsync(It.IsAny<Group>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async void ShouldThrowValidationExceptionOnCreateIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Group someGroup = CreateRandomGroup();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidGroupReferenceException =
                new InvalidGroupReferenceException(
                    message: "Invalid group reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var expectedGroupDependencyValidationException =
                new GroupDependencyValidationException(
                    message: "Group dependency validation occurred, please try again.",
                    innerException: invalidGroupReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Group> addGroupTask =
                this.groupService.AddGroupAsync(someGroup);

            GroupDependencyValidationException actualGroupDependencyValidationException =
                 await Assert.ThrowsAsync<GroupDependencyValidationException>(
                     addGroupTask.AsTask);

            // then
            actualGroupDependencyValidationException.Should().BeEquivalentTo(
                expectedGroupDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupAsync(It.IsAny<Group>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnCreateIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Group someGroup = CreateRandomGroup();

            var databaseUpdateException =
                new DbUpdateException();

            var failedGroupStorageException =
                new FailedGroupStorageException(
                    message: "Failed group storage error occurred, contact support.",
                    innerException: databaseUpdateException);

            var expectedGroupDependencyException =
                new GroupDependencyException(
                    message: "Group dependency error occurred, contact support.",
                    innerException: failedGroupStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Group> addGroupTask =
                this.groupService.AddGroupAsync(someGroup);

            GroupDependencyException actualGroupDependencyException =
                 await Assert.ThrowsAsync<GroupDependencyException>(
                     addGroupTask.AsTask);

            // then
            actualGroupDependencyException.Should().BeEquivalentTo(
                expectedGroupDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupAsync(It.IsAny<Group>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnCreateIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Group someGroup = CreateRandomGroup();
            var serviceException = new Exception();

            var failedGroupServiceException =
                new FailedGroupServiceException(
                    message: "Failed group service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedGroupServiceException =
                new GroupServiceException(
                    message: "Group service error occurred, contact support.",
                    innerException: failedGroupServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Group> addGroupTask =
                this.groupService.AddGroupAsync(someGroup);

            GroupServiceException actualGroupServiceException =
                 await Assert.ThrowsAsync<GroupServiceException>(
                     addGroupTask.AsTask);

            // then
            actualGroupServiceException.Should().BeEquivalentTo(
                expectedGroupServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupAsync(It.IsAny<Group>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}