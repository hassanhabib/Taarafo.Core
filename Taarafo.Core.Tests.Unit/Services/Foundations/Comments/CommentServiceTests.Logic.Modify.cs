// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.Comments;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        private async Task ShouldModifyCommentAsync()
        {
            // given
            int minuteInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            Comment randomComment = CreateRandomModifyComment(randomDate.AddMinutes(minuteInPast));
            Comment inputComment = randomComment.DeepClone();
            inputComment.UpdatedDate = randomDate;
            Comment storageComment = randomComment;
            Comment updatedComment = inputComment;
            Comment expectedComment = updatedComment.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(inputComment.Id))
                    .ReturnsAsync(storageComment);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateCommentAsync(inputComment))
                    .ReturnsAsync(updatedComment);

            // when
            Comment actualComment =
                await this.commentService.ModifyCommentAsync(inputComment);

            // then
            actualComment.Should().BeEquivalentTo(expectedComment);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(inputComment.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCommentAsync(inputComment),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}