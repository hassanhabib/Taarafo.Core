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
        public async Task ShouldModifyGroupPostAsync()
        {
            //given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            GroupPost randomGroupPost = CreateRandomModifyGroupPost(randomDate);
            GroupPost inputGroupPost = randomGroupPost;
            GroupPost storageGroupPost = inputGroupPost.DeepClone();
            GroupPost updatedGroupPost = inputGroupPost;
            GroupPost exceptedGroupPost = updatedGroupPost.DeepClone();
            Guid groupId = inputGroupPost.GroupId;
            Guid postId = inputGroupPost.PostId;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(groupId, postId))
                    .ReturnsAsync(storageGroupPost);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGroupPostAsync(inputGroupPost))
                    .ReturnsAsync(updatedGroupPost);

            //when
            GroupPost actualGroupPost =
                await this.groupPostService.ModifyGroupPostAsync(inputGroupPost);

            //then
            actualGroupPost.Should().BeEquivalentTo(exceptedGroupPost);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGroupPostAsync(inputGroupPost), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(groupId, postId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
