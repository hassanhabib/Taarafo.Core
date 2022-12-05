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
		public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
		{
			// given
			Guid invalidCommentId = Guid.Empty;

			var invalidCommentException =
				new InvalidCommentException();

			invalidCommentException.AddData(
				key: nameof(Comment.Id),
				values: "Id is required");

			var expectedCommentValidationException =
				new CommentValidationException(invalidCommentException);

			// when
			ValueTask<Comment> removeCommentByIdTask =
				this.commentService.RemoveCommentByIdAsync(invalidCommentId);

			CommentValidationException actualCommentValidationException =
			   await Assert.ThrowsAsync<CommentValidationException>(
				   removeCommentByIdTask.AsTask);

			// then
			actualCommentValidationException.Should().BeEquivalentTo(
				expectedCommentValidationException);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedCommentValidationException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteCommentAsync(It.IsAny<Comment>()),
					Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowNotFoundExceptionOnRemoveIfCommentIsNotFoundAndLogItAsync()
		{
			// given
			Guid inputCommentId = Guid.NewGuid();
			Comment noComment = null;

			var notFoundCommentException =
				new NotFoundCommentException(inputCommentId);

			var expectedCommentValidationException =
				new CommentValidationException(notFoundCommentException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectCommentByIdAsync(It.IsAny<Guid>()))
					.ReturnsAsync(noComment);

			// when
			ValueTask<Comment> removeCommentByIdTask =
				this.commentService.RemoveCommentByIdAsync(inputCommentId);

			CommentValidationException actualCommentValidationException =
			   await Assert.ThrowsAsync<CommentValidationException>(
				   removeCommentByIdTask.AsTask);

			// then
			actualCommentValidationException.Should().BeEquivalentTo(
				expectedCommentValidationException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCommentByIdAsync(It.IsAny<Guid>()),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedCommentValidationException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteCommentAsync(It.IsAny<Comment>()),
					Times.Never);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
