// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.GroupPosts;
using Taarafo.Core.Models.GroupPosts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupPosts
{
    public partial class GroupPostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidGroupId = Guid.Empty;
            Guid invalidPostId = Guid.Empty;

            var invalidGroupPostException = new InvalidGroupPostException();

            invalidGroupPostException.AddData(
                key: nameof(GroupPost.GroupId),
                values: "Id is required");

            invalidGroupPostException.AddData(
                key: nameof(GroupPost.PostId),
                values: "Id is required");

            var expectedGroupPostValidationException =
                new GroupPostValidationException(invalidGroupPostException);

            // when
            ValueTask<GroupPost> removeGroupPostByIdTask =
                this.groupPostService.RemoveGroupPostByIdAsync(invalidGroupId, invalidPostId);

            GroupPostValidationException actualGroupPostValidationException =
                await Assert.ThrowsAsync<GroupPostValidationException>(
                    removeGroupPostByIdTask.AsTask);

            // then
            actualGroupPostValidationException.Should()
                .BeEquivalentTo(expectedGroupPostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGroupPostAsync(It.IsAny<GroupPost>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfGroupPostIsNotFoundAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            GroupPost randomGroupPost = CreateRandomGroupPost(randomDateTime);
            Guid inputGroupId = randomGroupPost.GroupId;
            Guid inputPostld = randomGroupPost.PostId;
            GroupPost nullStorageGroupPost = null;

            var notFoundGroupPostException =
                new NotFoundGroupPostException(inputGroupId, inputPostld);

            var expectedGroupPostValidationException =
                new GroupPostValidationException(notFoundGroupPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(inputGroupId, inputPostld))
                    .ReturnsAsync(nullStorageGroupPost);

            //when
            ValueTask<GroupPost> removeGroupPostTask =
                this.groupPostService.RemoveGroupPostByIdAsync(inputGroupId, inputPostld);

            GroupPostValidationException actualGroupPostValidationException =
                await Assert.ThrowsAsync<GroupPostValidationException>(
                    removeGroupPostTask.AsTask);

            //then
            actualGroupPostValidationException.Should().BeEquivalentTo(
                expectedGroupPostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGroupPostAsync(It.IsAny<GroupPost>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
