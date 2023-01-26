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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
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

            var expectedGroupPostValidationException = new
                GroupPostValidationException(invalidGroupPostException);

            // when
            ValueTask<GroupPost> retrieveGroupPostByIdTask =
                this.groupPostService.RetrieveGroupPostByIdAsync(invalidGroupId, invalidPostId);

            GroupPostValidationException actualGroupPostValidationException =
                await Assert.ThrowsAsync<GroupPostValidationException>(
                    retrieveGroupPostByIdTask.AsTask);

            // then
            actualGroupPostValidationException.Should().BeEquivalentTo(expectedGroupPostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(invalidGroupId, invalidPostId),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfTeamIsNotFoundAndLogItAsync()
        {
            //given
            Guid someGroupId = Guid.NewGuid();
            Guid somePostId = Guid.NewGuid();
            GroupPost noGroupPost = null;

            var notFoundGroupPostException =
                new NotFoundGroupPostException(someGroupId, somePostId);

            var expectedGroupPostValidationException =
                new GroupPostValidationException(notFoundGroupPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupPostByIdAsync(someGroupId, somePostId))
                    .ReturnsAsync(noGroupPost);

            //when
            ValueTask<GroupPost> retrieveGroupPostByIdTask =
                this.groupPostService.RetrieveGroupPostByIdAsync(someGroupId, somePostId);

            GroupPostValidationException actualGroupPostValidationException =
                await Assert.ThrowsAsync<GroupPostValidationException>(
                    retrieveGroupPostByIdTask.AsTask);

            // then
            actualGroupPostValidationException.Should().BeEquivalentTo(expectedGroupPostValidationException);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectGroupPostByIdAsync(someGroupId, somePostId),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}