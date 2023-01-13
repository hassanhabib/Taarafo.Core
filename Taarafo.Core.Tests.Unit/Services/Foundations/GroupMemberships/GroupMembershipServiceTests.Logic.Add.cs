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
        public async Task ShouldAddGroupMembershipAsync()
        {
            // given
            GroupMembership randomGroupMembership = CreateRandomGroupMembership();
            GroupMembership inputGroupMembership = randomGroupMembership;
            GroupMembership storageGroupMembership = inputGroupMembership;
            GroupMembership expectedGroupMembership = storageGroupMembership.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGroupMembershipAsync(inputGroupMembership))
                    .ReturnsAsync(storageGroupMembership);

            // when
            GroupMembership actualGroupMembership =
                await this.groupMembershipService.AddGroupMembershipAsync(inputGroupMembership);

            // then
            actualGroupMembership.Should().BeEquivalentTo(expectedGroupMembership);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupMembershipAsync(inputGroupMembership),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
