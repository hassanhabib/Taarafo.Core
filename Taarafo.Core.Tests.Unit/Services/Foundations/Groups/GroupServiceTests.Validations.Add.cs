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
        public async Task ShouldThrowExceptionOnCreateIfGroupIsNullAndLogItAsync()
        {
            // given
            Group nullGroup = null;

            var nullGroupException =
                new NullGroupException();

            var expectedGroupValidationException =
                new GroupValidationException(nullGroupException);

            // when
            ValueTask<Group> addGroupTask =
                this.groupService.CreateGroupAsync(nullGroup);

            // then
            await Assert.ThrowsAsync<GroupValidationException>(() =>
                addGroupTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnCreateIfGroupIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidGroup = new Group
            {
                Name = invalidText,
                Description = invalidText
            };

            var invalidGroupException =
                new InvalidGroupException();

            invalidGroupException.AddData(
                key: nameof(Group.Id),
                values: "Id is required");

            invalidGroupException.AddData(
                key: nameof(Group.Name),
                values: "Text is required");

            invalidGroupException.AddData(
                key: nameof(Group.Description),
                values: "Text is required");

            invalidGroupException.AddData(
                key: nameof(Group.CreatedDate),
                values: "Date is required");

            invalidGroupException.AddData(
                key: nameof(Group.UpdatedDate),
                values: "Date is required");

            var expectedGroupValidationException =
                new GroupValidationException(invalidGroupException);

            // when
            ValueTask<Group> addGroupTask =
                this.groupService.CreateGroupAsync(invalidGroup);

            // then
            await Assert.ThrowsAsync<GroupValidationException>(() =>
                addGroupTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupAsync(invalidGroup),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            Group randomGroup = CreateRandomGroup();
            Group invalidGroup = randomGroup;

            invalidGroup.UpdatedDate =
                invalidGroup.CreatedDate.AddDays(randomNumber);

            var invalidGroupException =
                new InvalidGroupException();

            invalidGroupException.AddData(
                key: nameof(Group.UpdatedDate),
                values: $"Date is not the same as {nameof(Group.CreatedDate)}");

            var expectedGroupValidationException =
                new GroupValidationException(invalidGroupException);

            // when
            ValueTask<Group> addGroupTask =
                this.groupService.CreateGroupAsync(invalidGroup);

            // then
            await Assert.ThrowsAsync<GroupValidationException>(() =>
               addGroupTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupAsync(It.IsAny<Group>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}