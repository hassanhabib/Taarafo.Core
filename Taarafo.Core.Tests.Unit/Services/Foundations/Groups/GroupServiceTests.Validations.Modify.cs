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
		public async Task ShouldThrowValidationExceptionOnUpdateIfGroupIsNullAndLogItAsync()
		{
			// given
			Group nullGroup = null;
			var nullGroupException = new NullGroupException();

			var expectedGroupValidationException =
				new GroupValidationException(nullGroupException);

			// when
			ValueTask<Group> modifyGroupTask =
				this.groupService.ModifyGroupAsync(nullGroup);

			// then
			await Assert.ThrowsAsync<GroupValidationException>(() =>
				modifyGroupTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedGroupValidationException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectGroupByIdAsync(It.IsAny<Guid>()),
					Times.Never);

			this.storageBrokerMock.Verify(broker =>
				broker.UpdateGroupAsync(It.IsAny<Group>()),
					Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		public async Task ShouldThrowValidationExceptionOnUpdateIfGroupIsInvalidAndLogItAsync(
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
				values: new[]
					{
						"Date is required",
						$"Date is the same as {nameof(Group.CreatedDate)}",
						"Date is not recent"
					});

			var expectedGroupValidationException =
				new GroupValidationException(invalidGroupException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Returns(GetRandomDateTime);

			// when
			ValueTask<Group> updateGroupTask =
				this.groupService.ModifyGroupAsync(invalidGroup);

			// then
			await Assert.ThrowsAsync<GroupValidationException>(() =>
				updateGroupTask.AsTask());

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedGroupValidationException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.UpdateGroupAsync(invalidGroup),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowValidationExceptionOnUpdateIfCreateAndUpdateDatesIsSameAndLogItAsync()
		{
			// given
			DateTimeOffset randomDateTime = GetRandomDateTime();
			Group randomGroup = CreateRandomGroup(randomDateTime);
			Group invalidGroup = randomGroup;
			var invalidGroupException = new InvalidGroupException();

			invalidGroupException.AddData(
				key: nameof(Group.UpdatedDate),
				values: $"Date is the same as {nameof(Group.CreatedDate)}");

			var expectedGroupValidationException =
				new GroupValidationException(invalidGroupException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Returns(randomDateTime);

			// when
			ValueTask<Group> updateGroupTask =
				this.groupService.ModifyGroupAsync(invalidGroup);

			// then
			await Assert.ThrowsAsync<GroupValidationException>(() =>
			   updateGroupTask.AsTask());

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedGroupValidationException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertGroupAsync(It.IsAny<Group>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Theory]
		[MemberData(nameof(MinutesBeforeOrAfter))]
		public async Task ShouldThrowValidationExceptionOnUpdateIfUpdatedDateIsNotRecentAndLogItAsync(int minutesBeforeOrAfter)
		{
			// given
			DateTimeOffset dateTime = GetRandomDateTimeOffset();
			Group randomGroup = CreateRandomGroup(dateTime);
			Group inputGroup = randomGroup;
			inputGroup.UpdatedDate = dateTime.AddMinutes(minutesBeforeOrAfter);

			var invalidGroupException =
				new InvalidGroupException();

			invalidGroupException.AddData(
				key: nameof(Group.UpdatedDate),
				values: "Date is not recent");

			var expectedGroupValidatonException =
				new GroupValidationException(invalidGroupException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Returns(dateTime);

			// when
			ValueTask<Group> modifyGroupTask =
				this.groupService.ModifyGroupAsync(inputGroup);

			// then
			await Assert.ThrowsAsync<GroupValidationException>(() =>
				modifyGroupTask.AsTask());

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedGroupValidatonException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectGroupByIdAsync(It.IsAny<Guid>()),
					Times.Never);

			this.storageBrokerMock.Verify(broker =>
				broker.UpdateGroupAsync(It.IsAny<Group>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowValidationExceptionOnModifyIfGroupDoesNotExistAndLogItAsync()
		{
			// given
			int randomNegativeMinutes = GetRandomNegativeNumber();
			DateTimeOffset dateTime = GetRandomDateTimeOffset();
			Group randomGroup = CreateRandomGroup(dateTime);
			Group nonExistGroup = randomGroup;
			nonExistGroup.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
			Group nullGroup = null;

			var notFoundGroupException =
				new NotFoundGroupException(nonExistGroup.Id);

			var expectedGroupValidationException =
				new GroupValidationException(notFoundGroupException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Returns(dateTime);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectGroupByIdAsync(nonExistGroup.Id))
					.ReturnsAsync(nullGroup);

			// when 
			ValueTask<Group> modifyGroupTask =
				this.groupService.ModifyGroupAsync(nonExistGroup);

			// then
			await Assert.ThrowsAsync<GroupValidationException>(() =>
				modifyGroupTask.AsTask());

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectGroupByIdAsync(nonExistGroup.Id),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedGroupValidationException))),
						Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}