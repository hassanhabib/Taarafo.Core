// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.GroupPosts;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupPosts
{
    public partial class GroupPostServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllGroupPosts()
        {
            //given
            IQueryable<GroupPost> randomGroupPosts = CreateRandomGroupPosts();
            IQueryable<GroupPost> storageGroupPosts = randomGroupPosts;
            IQueryable<GroupPost> expectedGroupPosts = storageGroupPosts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGroupPosts()).Returns(storageGroupPosts);

            //when
            IQueryable<GroupPost> actualGroupPosts = this.groupPostService.RetrieveAllGroupPosts();

            //then
            actualGroupPosts.Should().BeEquivalentTo(expectedGroupPosts);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGroupPosts(), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
