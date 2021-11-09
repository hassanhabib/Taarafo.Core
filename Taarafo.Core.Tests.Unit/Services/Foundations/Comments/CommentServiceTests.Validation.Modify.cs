// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.Comments;
using Taarafo.Core.Models.Comments.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCommentIsNullAndLogItAsync()
        {
            // given
            Comment nullComment = null;
            var nullCommentException = new NullCommentException();

            var expectedCommentValidationException =
                new CommentValidationException(nullCommentException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(nullComment);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                modifyCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedCommentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCommentAsync(It.IsAny<Comment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
