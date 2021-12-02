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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCommentAsync(It.IsAny<Comment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfCommentIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidComment = new Comment
            {
                Content = invalidText,
            };

            var invalidCommentException = new InvalidCommentException();

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
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(Comment.CreatedDate)}"
                });

            invalidCommentException.AddData(
                key: nameof(Comment.PostId),
                values: "Id is required");

            var expectedCommentValidationException =
                new CommentValidationException(invalidCommentException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(invalidComment);

            //then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                modifyCommentTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCommentAsync(It.IsAny<Comment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Comment randomComment = CreateRandomComment(randomDateTime);
            Comment invalidComment = randomComment;
            var invalidCommentException = new InvalidCommentException();

            invalidCommentException.AddData(
                key: nameof(Comment.UpdatedDate),
                values: $"Date is the same as {nameof(Comment.CreatedDate)}");

            var expectedCommentValidationException =
                new CommentValidationException(invalidCommentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(invalidComment);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                modifyCommentTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(invalidComment.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            Comment randomComment = CreateRandomComment(dateTime);
            randomComment.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidCommentException =
                new InvalidCommentException();

            invalidCommentException.AddData(
                key: nameof(Comment.UpdatedDate),
                values: "Date is not recent");

            var expectedCommentValidatonException =
                new CommentValidationException(invalidCommentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(dateTime);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(randomComment);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                modifyCommentTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCommentDoesNotExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            Comment randomComment = CreateRandomComment(dateTime);
            Comment nonExistComment = randomComment;
            nonExistComment.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Comment nullComment = null;

            var notFoundCommentException =
                new NotFoundCommentException(nonExistComment.Id);

            var expectedCommentValidationException =
                new CommentValidationException(notFoundCommentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(nonExistComment.Id))
                .ReturnsAsync(nullComment);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(dateTime);

            // when 
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(nonExistComment);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                modifyCommentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(nonExistComment.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            Comment randomComment = CreateRandomComment(randomDate);
            Comment invalidComment = randomComment;
            invalidComment.UpdatedDate = randomDate;
            Comment storageComment = randomComment.DeepClone();
            Guid commentId = invalidComment.Id;
            invalidComment.CreatedDate = storageComment.CreatedDate.AddMinutes(randomMinutes);
            var invalidCommentException = new InvalidCommentException();

            invalidCommentException.AddData(
                key: nameof(Comment.CreatedDate),
                values: $"Date is not the same as {nameof(Comment.CreatedDate)}");

            var expectedCommentValidationException =
                new CommentValidationException(invalidCommentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(commentId))
                .ReturnsAsync(storageComment);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDate);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(invalidComment);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                modifyCommentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(invalidComment.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCommentValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Comment randomComment = CreateRandomModifyComment(randomDateTime);
            Comment invalidComment = randomComment;

            Comment storageComment = randomComment.DeepClone();
            invalidComment.UpdatedDate = storageComment.UpdatedDate;

            var invalidCommentException = new InvalidCommentException();

            invalidCommentException.AddData(
                key: nameof(Comment.UpdatedDate),
                values: $"Date is the same as {nameof(Comment.UpdatedDate)}");

            var expectedCommentValidationException =
                new CommentValidationException(invalidCommentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(invalidComment.Id))
                .ReturnsAsync(storageComment);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(invalidComment);

            // then
            await Assert.ThrowsAsync<CommentValidationException>(() =>
                modifyCommentTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(invalidComment.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
