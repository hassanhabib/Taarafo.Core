// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.Comments;
using Taarafo.Core.Models.Comments.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        private async Task ShouldThrowValidationExceptionOnAddIfCommentIsNullAndLogItAsync()
        {
            // given
            Comment nullComment = null;

            var nullCommentException =
                new NullCommentException();

            var expectedCommentValidationException =
                new CommentValidationException(
                    message: "Comment validation errors occurred, please try again.",
                        innerException: nullCommentException);

            // when
            ValueTask<Comment> addCommentTask =
                this.commentService.AddCommentAsync(nullComment);

            CommentValidationException actualCommentValidationException =
               await Assert.ThrowsAsync<CommentValidationException>(
                   addCommentTask.AsTask);

            // then
            actualCommentValidationException.Should().BeEquivalentTo(
                expectedCommentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        private async Task ShouldThrowValidationExceptionOnAddIfCommentIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidComment = new Comment
            {
                Content = invalidText
            };

            var invalidCommentException =
                new InvalidCommentException();

            invalidCommentException.AddData(
                key: nameof(Comment.Id),
                values: "Id is required");

            invalidCommentException.AddData(
                key: nameof(Comment.Content),
                values: "Text is required");

            invalidCommentException.AddData(
                key: nameof(Comment.CreatedDate),
                values: "Date is required");

            invalidCommentException.AddData(
                key: nameof(Comment.UpdatedDate),
                values: "Date is required");

            invalidCommentException.AddData(
                key: nameof(Comment.PostId),
                values: "Id is required");

            var expectedCommentValidationException =
                new CommentValidationException(
                    message: "Comment validation errors occurred, please try again.",
                        innerException: invalidCommentException);

            // when
            ValueTask<Comment> addCommentTask =
                this.commentService.AddCommentAsync(invalidComment);

            CommentValidationException actualCommentValidationException =
               await Assert.ThrowsAsync<CommentValidationException>(
                   addCommentTask.AsTask);

            // then
            actualCommentValidationException.Should().BeEquivalentTo(
                expectedCommentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCommentAsync(It.IsAny<Comment>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Comment randomComment = CreateRandomComment(randomDateTimeOffset);
            Comment invalidComment = randomComment;

            invalidComment.UpdatedDate =
                invalidComment.CreatedDate.AddDays(randomNumber);

            var invalidCommentException =
                new InvalidCommentException();

            invalidCommentException.AddData(
                key: nameof(Comment.UpdatedDate),
                values: $"Date is not the same as {nameof(Comment.CreatedDate)}");

            var expectedCommentValidationException =
                new CommentValidationException(
                    message: "Comment validation errors occurred, please try again.",
                        innerException: invalidCommentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Comment> addCommentTask =
                this.commentService.AddCommentAsync(invalidComment);

            CommentValidationException actualCommentValidationException =
                await Assert.ThrowsAsync<CommentValidationException>(
                    addCommentTask.AsTask);

            // then
            actualCommentValidationException.Should().BeEquivalentTo(expectedCommentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCommentAsync(It.IsAny<Comment>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        private async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTime =
                GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTime.AddMinutes(minutesBeforeOrAfter);

            Comment randomComment = CreateRandomComment(invalidDateTime);
            Comment invalidComment = randomComment;

            var invalidCommentException =
                new InvalidCommentException();

            invalidCommentException.AddData(
                key: nameof(Comment.CreatedDate),
                values: "Date is not recent");

            var expectedCommentValidationException =
                new CommentValidationException(
                    message: "Comment validation errors occurred, please try again.",
                        innerException: invalidCommentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Comment> addCommentTask =
                this.commentService.AddCommentAsync(invalidComment);

            CommentValidationException actualCommentValidationException =
               await Assert.ThrowsAsync<CommentValidationException>(
                   addCommentTask.AsTask);

            // then
            actualCommentValidationException.Should().BeEquivalentTo(
                expectedCommentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCommentAsync(It.IsAny<Comment>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}