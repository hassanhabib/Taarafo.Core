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
        public async Task ShouldRetrieveGroupPostByIdAsync()
        {
            // given
            Guid randomGroupPostId = Guid.NewGuid();
            Guid inputGroupPostId = randomGroupPostId;
            GroupPost randomGroupPost = CreateRandomGroupPost();
            GroupPost storageGroupPost = randomGroupPost;
            GroupPost expectedGroupPost = storageGroupPost.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(inputGroupPostId))
                    .ReturnsAsync(storageGroupPost);

            // when
            GroupPost actualGroupPost =
                await this.groupPostService.RetrieveGroupPostByIdAsync(inputGroupPostId);

            // then
            actualGroupPost.Should().BeEquivalentTo(expectedGroupPost);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(inputGroupPostId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}