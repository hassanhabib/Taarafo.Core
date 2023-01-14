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
using Taarafo.Core.Models.GroupPosts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupPosts
{
    public partial class GroupPostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfGroupPostIsNullAndLogItAsync()
        {
            // given
            GroupPost nullGroupPost = null;
            var nullGroupPostException = new NullGroupPostException();

            var expectedGroupPostValidationException =
                new GroupPostValidationException(nullGroupPostException);

            // when
            ValueTask<GroupPost> modifyGroupPostTask =
                this.groupPostService.ModifyGroupPostAsync(nullGroupPost);

            GroupPostValidationException actualGroupPostValidationException =
                await Assert.ThrowsAsync<GroupPostValidationException>(
                    modifyGroupPostTask.AsTask);

            // then
            actualGroupPostValidationException.Should().BeEquivalentTo(expectedGroupPostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGroupPostAsync(It.IsAny<GroupPost>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null, null)]
        public async Task ShouldThrowValidationExceptionOnModifyIfGroupPostIsInvalidAndLogItAsync(Guid invalidGroupId, Guid invalidPostId)
        {
            // given 
            var invalidGroupPost = new GroupPost
            {
                GroupId = invalidGroupId,
                PostId = invalidPostId
            };

            var invalidGroupPostException = new InvalidGroupPostException();

            invalidGroupPostException.AddData(
                key: nameof(GroupPost.GroupId),
                values: "Id is required");

            invalidGroupPostException.AddData(
                key: nameof(GroupPost.PostId),
                values: "Id is required");

            var expectedGroupPostValidationException =
                new GroupPostValidationException(invalidGroupPostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(GetRandomDateTimeOffset);

            // when
            ValueTask<GroupPost> modifyGroupPostTask =
                this.groupPostService.ModifyGroupPostAsync(invalidGroupPost);

            GroupPostValidationException actualGroupPostValidationException =
                await Assert.ThrowsAsync<GroupPostValidationException>(
                    modifyGroupPostTask.AsTask);

            //then
            actualGroupPostValidationException.Should()
                .BeEquivalentTo(expectedGroupPostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGroupPostAsync(It.IsAny<GroupPost>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfGroupPostDoesNotExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetRandomNegativeNumber();
            GroupPost randomGroupPost = CreateRandomGroupPost();
            GroupPost nonExistGroupPost = randomGroupPost;
            GroupPost nullGroupPost = null;

            var notFoundGroupPostException =
                new NotFoundGroupPostException(nonExistGroupPost.GroupId,
                    nonExistGroupPost.PostId);

            var expectedGroupPostValidationException =
                new GroupPostValidationException(notFoundGroupPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(nonExistGroupPost.GroupId,
                    nonExistGroupPost.PostId)).ReturnsAsync(nullGroupPost);

            // when 
            ValueTask<GroupPost> modifyGroupPostTask =
                this.groupPostService.ModifyGroupPostAsync(nonExistGroupPost);

            GroupPostValidationException actualGroupPostValidationException =
                await Assert.ThrowsAsync<GroupPostValidationException>(modifyGroupPostTask.AsTask);

            // then
            actualGroupPostValidationException.Should().BeEquivalentTo(expectedGroupPostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(nonExistGroupPost.GroupId,
                    nonExistGroupPost.PostId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            GroupPost randomGroupPost = CreateRandomModifyGroupPost(randomDateTime);
            GroupPost invalidGroupPost = randomGroupPost.DeepClone();
            GroupPost storageGroupPost = invalidGroupPost.DeepClone();
            var invalidGroupPostException = new InvalidGroupPostException();
            Guid GroupId = invalidGroupPost.GroupId;
            Guid PostId = invalidGroupPost.PostId;


            var expectedGroupPostValidationException =
                new GroupPostValidationException(invalidGroupPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(GroupId, PostId)).ReturnsAsync(storageGroupPost);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<GroupPost> modifyGroupPostTask =
                this.groupPostService.ModifyGroupPostAsync(invalidGroupPost);
            GroupPostValidationException actualGroupPostValidationException =
                await Assert.ThrowsAsync<GroupPostValidationException>(modifyGroupPostTask.AsTask);

            // then
            actualGroupPostValidationException.Should().BeEquivalentTo(expectedGroupPostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(invalidGroupPost.GroupId,
                    invalidGroupPost.PostId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedGroupPostValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateIsSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            GroupPost randomGroupPost = CreateRandomModifyGroupPost(randomDateTime);
            GroupPost invalidGroupPost = randomGroupPost;
            GroupPost storageGroupPost = randomGroupPost.DeepClone();
     
            Guid groupId = invalidGroupPost.GroupId;
            Guid postId = invalidGroupPost.PostId;
            var invalidGroupPostException = new InvalidGroupPostException();

            var expectedGroupPostValidationException =
                new GroupPostValidationException(invalidGroupPostException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectGroupPostByIdAsync(invalidGroupPost.GroupId,
                   invalidGroupPost.PostId)).ReturnsAsync(storageGroupPost);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<GroupPost> modifyGroupPostTask = this.groupPostService.ModifyGroupPostAsync(invalidGroupPost);
            GroupPostValidationException actualGroupPostValidationException =
               await Assert.ThrowsAsync<GroupPostValidationException>(modifyGroupPostTask.AsTask);

            // then
            actualGroupPostValidationException.Should().BeEquivalentTo(expectedGroupPostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(groupId, postId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
