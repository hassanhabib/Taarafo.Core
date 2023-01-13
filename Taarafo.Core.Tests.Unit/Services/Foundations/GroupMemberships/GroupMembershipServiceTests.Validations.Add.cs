// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.GroupMemberships;
using Taarafo.Core.Models.GroupMemberships.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupMemberships
{
    public partial class GroupMembershipServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfGroupMembershipIsNullAndLogItAsync()
        {
            // given
            GroupMembership nullGroupMembership = null;

            var nullGroupMembershipException =
                new NullGroupMembershipException();

            var expectedGroupMembershipValidationException =
                new GroupMembershipValidationException(nullGroupMembershipException);

            // when
            ValueTask<GroupMembership> addGroupMembershipTask =
                this.groupMembershipService.AddGroupMembershipAsync(nullGroupMembership);

            GroupMembershipValidationException actualGroupMembershipValidationException =
                await Assert.ThrowsAsync<GroupMembershipValidationException>(
                    addGroupMembershipTask.AsTask);

            // then
            actualGroupMembershipValidationException.Should().BeEquivalentTo(
                expectedGroupMembershipValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupMembershipValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}