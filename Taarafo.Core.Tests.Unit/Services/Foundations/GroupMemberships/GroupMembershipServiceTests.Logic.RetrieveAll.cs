// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.GroupMemberships;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupMemberships
{
    public partial class GroupMembershipServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllGroupMembershipAsync()
        {
            // given
            IQueryable<GroupMembership> randomGoupMemberships = CreateRandomGroupMemberships();
            IQueryable<GroupMembership> storageGroupMemberships = randomGoupMemberships;
            IQueryable<GroupMembership> expectedGroupMemberships = storageGroupMemberships;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGroupMemberships())
                    .Returns(storageGroupMemberships);

            // when
            IQueryable<GroupMembership> actualGroupMemberships =
                this.groupMembershipService.RetrieveAllGroupMemberships();

            // then
            actualGroupMemberships.Should().BeEquivalentTo(expectedGroupMemberships);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGroupMemberships(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}