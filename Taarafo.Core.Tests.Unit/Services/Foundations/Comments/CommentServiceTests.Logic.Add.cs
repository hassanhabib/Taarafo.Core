using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Comments;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        public async Task ShouldAddCommentAsync()
        {
            // given
            Comment randomComment = CreateRandomComment();
            Comment inputComment = randomComment;
            Comment storageComment = inputComment;
            Comment expectedComment = storageComment.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCommentAsync(inputComment))
                    .ReturnsAsync(storageComment);

            // when
            Comment actualComment = await this.commentService
                .AddCommentAsync(inputComment);

            // then
            actualComment.Should().BeEquivalentTo(expectedComment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCommentAsync(inputComment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
