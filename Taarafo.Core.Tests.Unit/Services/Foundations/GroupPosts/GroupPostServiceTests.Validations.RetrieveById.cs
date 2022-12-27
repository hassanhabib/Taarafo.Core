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
            var invalidGroupPostId = Guid.Empty;

            var invalidGroupPostException = new InvalidGroupPostException();

            invalidGroupPostException.AddData(
                key: nameof(GroupPost.GroupId),
                values: "Id is required");
            invalidGroupPostException.AddData(
                key: nameof(GroupPost.PostId),
                values: "Id is required");

            var expectedGroupPostValidationException = new
                Models.GroupPosts.Exceptions.GroupPostValidationException(invalidGroupPostException);

            // when
            ValueTask<GroupPost> retrieveGroupPostByIdTask =
                this.groupPostService.RetrieveGroupPostByIdAsync(invalidGroupPostId);

            GroupPostValidationException actualGroupPostValidationException =
                await Assert.ThrowsAsync<GroupPostValidationException>(
                    retrieveGroupPostByIdTask.AsTask);

            // then
            actualGroupPostValidationException.Should().BeEquivalentTo(expectedGroupPostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupPostByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}