// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.Groups;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Groups
{
	public partial class GroupServiceTests
	{
		[Fact]
		public async void ShouldRemoveGroupByIdAsync()
		{
			// given
			Guid randomId = Guid.NewGuid();
			Guid inputGroupId = randomId;
			Group randomGroup = CreateRandomGroup();
			Group storageGroup = randomGroup;
			Group expectedInputGroup = storageGroup;
			Group deletedGroup = expectedInputGroup;
			Group expectedGroup = deletedGroup.DeepClone();

			this.storageBrokerMock.Setup(broker =>
				broker.SelectGroupByIdAsync(inputGroupId))
					.ReturnsAsync(storageGroup);

			this.storageBrokerMock.Setup(broker =>
				broker.DeleteGroupAsync(expectedInputGroup))
					.ReturnsAsync(deletedGroup);

			// when
			Group actualGroup = await this.groupService
				.RemoveGroupByIdAsync(inputGroupId);

			// then
			actualGroup.Should().BeEquivalentTo(expectedGroup);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectGroupByIdAsync(inputGroupId),
					Times.Once());

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteGroupAsync(expectedInputGroup),
					Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
