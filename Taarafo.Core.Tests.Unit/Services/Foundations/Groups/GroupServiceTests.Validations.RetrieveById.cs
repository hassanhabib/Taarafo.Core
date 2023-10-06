// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.Groups;
using Taarafo.Core.Models.Groups.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Groups
{
	public partial class GroupServiceTests
	{
		[Fact]
		public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
		{
			//given
			Guid invalidGroupId = Guid.Empty;

			var invalidGroupException =
				new InvalidGroupException();

			invalidGroupException.AddData(
				key: nameof(Group.Id),
				values: "Id is required");

			var expectedGroupValidationException =
				new GroupValidationException(invalidGroupException);

			//when
			ValueTask<Group> retrieveGroupByIdTask =
				this.groupService.RetrieveGroupByIdAsync(invalidGroupId);

            GroupValidationException actualGroupValidationException =
               await Assert.ThrowsAsync<GroupValidationException>(
                   retrieveGroupByIdTask.AsTask);

            //then
            actualGroupValidationException.Should().BeEquivalentTo(
				expectedGroupValidationException);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedGroupValidationException))),
						Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfGroupIsNotFoundAndLogItAsync()
		{
			//given
			Guid someGroupId = Guid.NewGuid();
			Group noGroup = null;

			var notFoundGroupException =
				new NotFoundGroupException(someGroupId);

			var expectedGroupValidationException =
				new GroupValidationException(notFoundGroupException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectGroupByIdAsync(It.IsAny<Guid>()))
					.ReturnsAsync(noGroup);

			//when
			ValueTask<Group> retrieveGroupByIdTask =
				this.groupService.RetrieveGroupByIdAsync(someGroupId);

            GroupValidationException actualGroupValidationException =
               await Assert.ThrowsAsync<GroupValidationException>(
                   retrieveGroupByIdTask.AsTask);

            //then
            actualGroupValidationException.Should().BeEquivalentTo(
				expectedGroupValidationException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectGroupByIdAsync(It.IsAny<Guid>()),
					Times.Once());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedGroupValidationException))),
						Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
