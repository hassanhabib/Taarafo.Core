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
        public async Task ShouldModifyGroupMembershipAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            GroupMembership randomGroupMembership = CreateRandomModifyGroupMembership(randomDateTime);
            GroupMembership inputGroupMembership = randomGroupMembership;
            GroupMembership storageGroupMembership = inputGroupMembership.DeepClone();
            GroupMembership updateGroupMembership = inputGroupMembership;
            GroupMembership expectedGroupMembership = updateGroupMembership.DeepClone();
            Guid groupMembershipId = inputGroupMembership.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupMembershipByIdAsync(groupMembershipId))
                    .ReturnsAsync(storageGroupMembership);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGroupMembershipAsync(inputGroupMembership))
                    .ReturnsAsync(updateGroupMembership);

            //when
            GroupMembership actualGroupMembership =
                await this.groupMembershipService.ModifyGroupMembershipAsync(inputGroupMembership);

            //then
            actualGroupMembership.Should().BeEquivalentTo(expectedGroupMembership);


            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupMembershipByIdAsync(groupMembershipId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGroupMembershipAsync(inputGroupMembership),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }
    }
}