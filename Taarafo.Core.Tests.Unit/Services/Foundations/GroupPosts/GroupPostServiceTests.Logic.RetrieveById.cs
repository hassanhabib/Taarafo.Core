// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.GroupPosts;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupPosts
{
    public partial class GroupPostServiceTests
    {
        [Fact]
        private async Task ShouldRetrieveGroupPostByIdAsync()
        {
            // given
            Guid randomGroupId = Guid.NewGuid();
            Guid randomPostId = Guid.NewGuid();
            Guid inputGroupId = randomGroupId;
            Guid inputPostId = randomPostId;
            GroupPost randomGroupPost = CreateRandomGroupPost();
            GroupPost storageGroupPost = randomGroupPost;
            GroupPost expectedGroupPost = storageGroupPost.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(inputGroupId, inputPostId))
                    .ReturnsAsync(storageGroupPost);

            // when
            GroupPost actualGroupPost =
                await this.groupPostService.RetrieveGroupPostByIdAsync(
                    inputGroupId, inputPostId);

            // then
            actualGroupPost.Should().BeEquivalentTo(expectedGroupPost);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(inputGroupId, inputPostId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}