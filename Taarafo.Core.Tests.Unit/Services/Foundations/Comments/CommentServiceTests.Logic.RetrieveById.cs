// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System;
using System.Threading.Tasks;
using Taarafo.Core.Models.Comments;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveCommentByIdAsync()
        {
            // given
            Comment randomComment = CreateRandomComment();
            Comment storageComment = randomComment;
            Comment expectedComment = storageComment.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(randomComment.Id))
                    .ReturnsAsync(storageComment);

            // when
            Comment actualComment =
                await this.commentService.RetrieveCommentByIdAsync(randomComment.Id);

            // then
            actualComment.Should().BeEquivalentTo(expectedComment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(randomComment.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
