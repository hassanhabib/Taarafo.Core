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
        private async Task ShouldThrowCriticalDependencyExceptionOnUpdateIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Group randomGroup = CreateRandomGroup();
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
            ValueTask<Group> modifyGroupTask =
                this.groupService.ModifyGroupAsync(randomGroup);

            GroupDependencyException actualGroupDependencyException =
                 await Assert.ThrowsAsync<GroupDependencyException>(
                    modifyGroupTask.AsTask);

            // then
            actualGroupDependencyException.Should().BeEquivalentTo(
                expectedGroupDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(randomGroup.Id),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGroupAsync(randomGroup),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async void ShouldThrowValidationExceptionOnUpdateIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Group someGroup =
                CreateRandomGroup();

            Group foreignKeyConflictedGroup =
                someGroup;

            string randomMessage =
                GetRandomMessage();

            string exceptionMessage =
                randomMessage;

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
            ValueTask<Group> modifyGroupTask =
                this.groupService.ModifyGroupAsync(foreignKeyConflictedGroup);

            GroupDependencyValidationException actualGroupDependencyValidationException =
                 await Assert.ThrowsAsync<GroupDependencyValidationException>(
                    modifyGroupTask.AsTask);

            // then
            actualGroupDependencyValidationException.Should().BeEquivalentTo(
                expectedGroupDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(foreignKeyConflictedGroup.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGroupDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGroupAsync(foreignKeyConflictedGroup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Group randomGroup = CreateRandomGroup();
            var databaseUpdateException = new DbUpdateException();

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
            ValueTask<Group> modifyGroupTask =
                this.groupService.ModifyGroupAsync(randomGroup);

            GroupDependencyException actualGroupDependencyException =
                 await Assert.ThrowsAsync<GroupDependencyException>(
                     modifyGroupTask.AsTask);

            // then
            actualGroupDependencyException.Should().BeEquivalentTo(
                expectedGroupDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(randomGroup.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGroupAsync(randomGroup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationOnUpdateIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Group randomGroup = CreateRandomGroup();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedGroupException =
                new LockedGroupException(
                    message: "Locked group record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedGroupDependencyValidationException =
                new GroupDependencyValidationException(
                    message: "Group dependency validation occurred, please try again.",
                    innerException: lockedGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Group> modifyGroupTask =
                this.groupService.ModifyGroupAsync(randomGroup);

            GroupDependencyValidationException actualGroupDependencyValidationException =
                 await Assert.ThrowsAsync<GroupDependencyValidationException>(
                     modifyGroupTask.AsTask);

            // then
            actualGroupDependencyValidationException.Should().BeEquivalentTo(
                expectedGroupDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(randomGroup.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGroupAsync(randomGroup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnUpdateIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Group randomGroup = CreateRandomGroup();
            var serviceException = new Exception();

            var failedGroupException =
                new FailedGroupServiceException(
                    message: "Failed group service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedGroupServiceException =
                new GroupServiceException(
                    message: "Group service error occurred, contact support.",
                    innerException: failedGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Group> modifyGroupTask =
                this.groupService.ModifyGroupAsync(randomGroup);

            GroupServiceException actualGroupServiceException =
                 await Assert.ThrowsAsync<GroupServiceException>(
                     modifyGroupTask.AsTask);

            // then
            actualGroupServiceException.Should().BeEquivalentTo(
                expectedGroupServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(randomGroup.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGroupAsync(randomGroup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}