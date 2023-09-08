// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        private async Task ShouldUpdateGroupAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            Group randomGroup = CreateRandomGroup(randomDate);
            Group inputGroup = randomGroup;
            inputGroup.UpdatedDate = randomDate.AddMinutes(1);
            Group storageGroup = inputGroup;
            Group updatedGroup = inputGroup;
            Group expectedGroup = updatedGroup.DeepClone();
            Guid inputGroupId = inputGroup.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupByIdAsync(inputGroupId))
                        .ReturnsAsync(storageGroup);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGroupAsync(inputGroup))
                        .ReturnsAsync(updatedGroup);

            // when
            Group actualGroup =
                await this.groupService.ModifyGroupAsync(inputGroup);

            // then
            actualGroup.Should().BeEquivalentTo(expectedGroup);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(inputGroupId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGroupAsync(inputGroup),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}