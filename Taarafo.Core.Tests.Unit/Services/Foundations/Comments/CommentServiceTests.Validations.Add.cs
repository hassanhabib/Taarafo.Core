using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Comments;
using Taarafo.Core.Models.Comments.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCommentIsNullAndLogItAsync()
        {
            // given
            Comment nullComment = null;

            var nullCommentException =
                new NullCommentException();

            var expectedCommentValidationException =
                new CommentValidationException(nullCommentException);

            // when
            ValueTask<Comment> addCommentTask =
                this.commentService.AddCommentAsync(nullComment);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                addCommentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedCommentValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
