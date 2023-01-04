// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
