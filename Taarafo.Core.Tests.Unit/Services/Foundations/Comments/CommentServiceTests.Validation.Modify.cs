// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
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
        private async Task ShouldThrowValidationExceptionOnModifyIfCommentIsNullAndLogItAsync()
        {
            // given
            Comment nullComment = null;
            var nullCommentException = new NullCommentException();

            var expectedCommentValidationException =
                new CommentValidationException(
                    message: "Comment validation errors occurred, please try again.",
                        innerException: nullCommentException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(nullComment);

            CommentValidationException actualCommentValidationException =
                await Assert.ThrowsAsync<CommentValidationException>(
                    modifyCommentTask.AsTask);

            // then
            actualCommentValidationException.Should().BeEquivalentTo(
                expectedCommentValidationException);

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
        private async Task ShouldThrowValidationExceptionOnModifyIfCommentIsInvalidAndLogItAsync(string invalidText)
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
                new CommentValidationException(
                    message: "Comment validation errors occurred, please try again.",
                        innerException: invalidCommentException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(invalidComment);

            CommentValidationException actualCommentValidationException =
               await Assert.ThrowsAsync<CommentValidationException>(
                   modifyCommentTask.AsTask);

            // then
            actualCommentValidationException.Should().BeEquivalentTo(
                expectedCommentValidationException);

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
        private async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
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
                new CommentValidationException(
                    message: "Comment validation errors occurred, please try again.",
                        innerException: invalidCommentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(invalidComment);

            CommentValidationException actualCommentValidationException =
               await Assert.ThrowsAsync<CommentValidationException>(
                   modifyCommentTask.AsTask);

            // then
            actualCommentValidationException.Should().BeEquivalentTo(
                expectedCommentValidationException);

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
        private async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
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

            var expectedCommentValidationException =
                new CommentValidationException(
                    message: "Comment validation errors occurred, please try again.",
                        innerException: invalidCommentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(dateTime);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(randomComment);

            CommentValidationException actualCommentValidationException =
               await Assert.ThrowsAsync<CommentValidationException>(
                   modifyCommentTask.AsTask);

            // then
            actualCommentValidationException.Should().BeEquivalentTo(
                expectedCommentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfCommentDoesNotExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            Comment randomComment = CreateRandomComment(dateTime);
            Comment nonExistComment = randomComment;
            nonExistComment.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Comment nullComment = null;
            var invalidCommentException = new InvalidOperationException();

            var notFoundCommentException =
                new NotFoundCommentException(
                     message: $"Comment with id: {nonExistComment.Id} not found, please contact support.",
                     innerException: invalidCommentException);

            var expectedCommentValidationException =
                new CommentValidationException(
                    message: "Comment validation errors occurred, please try again.",
                        innerException: notFoundCommentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(nonExistComment.Id))
                    .ReturnsAsync(nullComment);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(dateTime);

            // when 
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(nonExistComment);

            CommentValidationException actualCommentValidationException =
               await Assert.ThrowsAsync<CommentValidationException>(
                   modifyCommentTask.AsTask);

            // then
            actualCommentValidationException.Should().BeEquivalentTo(
                expectedCommentValidationException);

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
        private async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            Comment randomComment = CreateRandomModifyComment(randomDate);
            Comment invalidComment = randomComment.DeepClone();
            Comment storageComment = randomComment.DeepClone();
            storageComment.CreatedDate = storageComment.CreatedDate.AddMinutes(randomMinutes);
            storageComment.UpdatedDate = storageComment.UpdatedDate.AddMinutes(randomMinutes);
            Guid commentId = invalidComment.Id;

            var invalidCommentException =
                new InvalidCommentException();

            invalidCommentException.AddData(
                key: nameof(Comment.CreatedDate),
                values: $"Date is not the same as {nameof(Comment.CreatedDate)}");

            var expectedCommentValidationException =
                new CommentValidationException(
                    message: "Comment validation errors occurred, please try again.",
                        innerException: invalidCommentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(commentId))
                .ReturnsAsync(storageComment);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(invalidComment);

            CommentValidationException actualCommentValidationException =
                await Assert.ThrowsAsync<CommentValidationException>(() =>
                    modifyCommentTask.AsTask());

            // then
            actualCommentValidationException.Should()
                .BeEquivalentTo(expectedCommentValidationException);

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
        private async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
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
                new CommentValidationException(
                    message: "Comment validation errors occurred, please try again.",
                        innerException: invalidCommentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(invalidComment.Id))
                .ReturnsAsync(storageComment);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(invalidComment);

            CommentValidationException actualCommentValidationException =
               await Assert.ThrowsAsync<CommentValidationException>(
                   modifyCommentTask.AsTask);

            // then
            actualCommentValidationException.Should().BeEquivalentTo(
                expectedCommentValidationException);

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

        [Fact]
        private async void ShouldThrowValidationExceptionOnModifyIfForeignKeyReferenceErrorOccursAndLogItAsync()
        {
            // given
            Comment foreignKeyConflictedComment = CreateRandomComment();
            ForeignKeyConstraintConflictException foreignKeyException = GetForeignKeyException();

            var foreignKeyConstraintConflictException =
                new ForeignKeyCommentReferenceException(foreignKeyException);

            var invalidCommentReferenceException =
                new InvalidCommentReferenceException(
                    message: "Invalid comment reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

            var expectedCommentDependencyValidationException =
                new CommentDependencyValidationException(
                    message: "Comment dependency validation occurred, please try again.",
                        innerException: invalidCommentReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Comment> modifyCommentTask =
                this.commentService.ModifyCommentAsync(foreignKeyConflictedComment);

            CommentDependencyValidationException actualCommentDependencyValidationException =
                await Assert.ThrowsAsync<CommentDependencyValidationException>(
                    modifyCommentTask.AsTask);

            // then
            actualCommentDependencyValidationException.Should().BeEquivalentTo(
                expectedCommentDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(foreignKeyConflictedComment.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCommentDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCommentAsync(foreignKeyConflictedComment),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}