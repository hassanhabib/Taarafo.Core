// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.GroupMemberships;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupMemberships
{
    public partial class GroupMembershipServiceTests
    {
        [Fact]
        private async Task ShouldRetrieveGroupMembershipByIdAsync()
        {
            // given
            GroupMembership randomGroupMembership = CreateRandomGroupMembership();
            GroupMembership storageGroupMembership = randomGroupMembership;
            GroupMembership expectedGroupMembership = storageGroupMembership.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupMembershipByIdAsync(randomGroupMembership.Id))
                    .ReturnsAsync(storageGroupMembership);

            // when
            GroupMembership actualGroupMembership =
                await this.groupMembershipService.RetrieveGroupMembershipByIdAsync(
                    randomGroupMembership.Id);

            // then
            actualGroupMembership.Should().BeEquivalentTo(expectedGroupMembership);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupMembershipByIdAsync(randomGroupMembership.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}