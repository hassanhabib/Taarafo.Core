// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using Taarafo.Core.Models.Groups;
using Taarafo.Core.Models.Groups.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Groups
{
    public partial class GroupServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidGroupId = Guid.Empty;

            var invalidGroupException =
                new InvalidGroupException();

            invalidGroupException.AddData(
                key: nameof(Group.Id),
                values: "Id is required");

            var expectedGroupValidationException =
                new GroupValidationException(invalidGroupException);

            // when
            ValueTask<Group> removeGroupByIdTask =
                this.groupService.RemoveGroupByIdAsync(invalidGroupId);

            // then
            await Assert.ThrowsAsync<GroupValidationException>(() =>
                removeGroupByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGroupAsync(It.IsAny<Group>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveGroupByIdIsNotFoundAndLogItAsync()
        {
            // given
            Guid inputGroupId = Guid.NewGuid();
            Group noGroup = null;

            var notFoundGroupException =
                new NotFoundGroupException(inputGroupId);

            var expectedGroupValidationException =
                new GroupValidationException(notFoundGroupException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noGroup);

            // when
            ValueTask<Group> removeGroupByIdTask =
                this.groupService.RemoveGroupByIdAsync(inputGroupId);

            // then
            await Assert.ThrowsAsync<GroupValidationException>(() =>
                removeGroupByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))),
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
