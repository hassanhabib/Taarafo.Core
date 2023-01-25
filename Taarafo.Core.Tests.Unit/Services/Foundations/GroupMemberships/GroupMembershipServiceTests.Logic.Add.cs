// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
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
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            GroupMembership randomGroupMembership = CreateRandomGroupMembership(randomDateTime);
            GroupMembership inputGroupMembership = randomGroupMembership;
            GroupMembership storageGroupMembership = inputGroupMembership;
            GroupMembership expectedGroupMembership = storageGroupMembership.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGroupMembershipAsync(inputGroupMembership))
                    .ReturnsAsync(storageGroupMembership);

            // when
            GroupMembership actualGroupMembership =
                await this.groupMembershipService.AddGroupMembershipAsync(inputGroupMembership);

            // then
            actualGroupMembership.Should().BeEquivalentTo(expectedGroupMembership);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupMembershipAsync(inputGroupMembership),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
