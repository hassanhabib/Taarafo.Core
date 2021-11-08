// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.Comments;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        public void ShouldReturnComments()
        {
            // given
            IQueryable<Comment> randomComments = CreateRandomComments();
            IQueryable<Comment> storageComments = randomComments;
            IQueryable<Comment> expectedComments = storageComments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllComments())
                    .Returns(storageComments);

            // when
            IQueryable<Comment> actualComments =
                this.commentService.RetrieveAllComments();

            // then
            actualComments.Should().BeEquivalentTo(expectedComments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllComments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
