// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.Groups;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Groups
{
    public partial class GroupServiceTests
    {
        [Fact]
        private void ShouldRetrieveAllGroups()
        {
            // given
            IQueryable<Group> randomGroups = CreateRandomGroups();
            IQueryable<Group> storageGroups = randomGroups;
            IQueryable<Group> expectedGroups = storageGroups;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGroups())
                    .Returns(storageGroups);

            // when
            IQueryable<Group> actualGroups =
                this.groupService.RetrieveAllGroups();

            // then
            actualGroups.Should().BeEquivalentTo(expectedGroups);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGroups(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}