// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someGroupId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedGroupStorageException =
                new FailedGroupStorageException(sqlException);

            var expectedGroupDependencyException =
                new GroupDependencyException(failedGroupStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Group> removeGroupByIdTask =
                this.groupService.RemoveGroupByIdAsync(someGroupId);

            // then
            await Assert.ThrowsAsync<GroupDependencyException>(() =>
                removeGroupByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGroupAsync(It.IsAny<Group>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someGroupId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedGroupException =
                new LockedGroupException(databaseUpdateConcurrencyException);

            var expectedGroupDependencyValidationException =
                new GroupDependencyException(lockedGroupException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Group> removeGroupByIdTask =
                this.groupService.RemoveGroupByIdAsync(someGroupId);

            // then
            await Assert.ThrowsAsync<GroupDependencyException>(() =>
                removeGroupByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGroupAsync(It.IsAny<Group>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
