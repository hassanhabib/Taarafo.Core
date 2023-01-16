// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
        public async void ShouldRetrieveGroupByIdAsync()
        {
            //given
            Group someGroup = CreateRandomGroup();
            Group storageGroup = someGroup;
            Group expectedGroup = storageGroup.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupByIdAsync(someGroup.Id))
                    .ReturnsAsync(storageGroup);

            //when
            Group actualGroup =
                await this.groupService.RetrieveGroupByIdAsync(someGroup.Id);

            //then
            actualGroup.Should().BeEquivalentTo(expectedGroup);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(someGroup.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
