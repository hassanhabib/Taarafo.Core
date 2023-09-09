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
        private async Task ShouldThrowValidationExceptionOnModifyIfGroupPostIsNullAndLogItAsync()
        {
            // given
            GroupPost nullGroupPost = null;
            var nullGroupPostException = new NullGroupPostException();

            var expectedGroupPostValidationException =
                new GroupPostValidationException(
                    message: "Group post validation error occurred, please try again.",
                    innerException: nullGroupPostException);

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
        private async Task ShouldThrowValidationExceptionOnModifyIfGroupPostIsInvalidAndLogItAsync(Guid invalidGroupId, Guid invalidPostId)
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
                new GroupPostValidationException(
                    message: "Group post validation error occurred, please try again.",
                    innerException: invalidGroupPostException);

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
        private async Task ShouldThrowValidationExceptionOnModifyIfGroupPostDoesNotExistAndLogItAsync()
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
                new GroupPostValidationException(
                    message: "Group post validation error occurred, please try again.",
                    innerException: notFoundGroupPostException);

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
    }
}