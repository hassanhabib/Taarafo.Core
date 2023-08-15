// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.Comments.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Comments
{
	public partial class CommentServiceTests
	{
		[Fact]
		private void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
		{
			// given
			SqlException sqlException = GetSqlException();

			var failedStorageException =
				new FailedCommentStorageException(
					message: "Failed comment storage error occurred, contact support.",
						innerException: sqlException);

			var expectedCommentDependencyException =
				new CommentDependencyException(
					message: "Comment dependency error occurred, contact support.",
						innerException: failedStorageException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectAllComments())
					.Throws(sqlException);

			// when
			Action retrieveAllCommentsAction = () =>
				this.commentService.RetrieveAllComments();

			CommentDependencyException actualCommentDependencyException =
				Assert.Throws<CommentDependencyException>(
					retrieveAllCommentsAction);

			// then
			actualCommentDependencyException.Should().BeEquivalentTo(
				expectedCommentDependencyException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAllComments(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(
					expectedCommentDependencyException))),
						Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		private void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
		{
			// given
			string exceptionMessage = GetRandomMessage();
			var serviceException = new Exception(exceptionMessage);

			var failedCommentServiceException =
				new FailedCommentServiceException(
					message: "Failed comment service occurred, please contact support",
						innerException: serviceException);

			var expectedCommentServiceException =
				new CommentServiceException(
					message: "Comment service error occurred, contact support.",
						innerException: failedCommentServiceException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectAllComments())
					.Throws(serviceException);

			// when
			Action retrieveAllCommentsAction = () =>
				this.commentService.RetrieveAllComments();

			CommentServiceException actualCommentServiceException =
				Assert.Throws<CommentServiceException>(retrieveAllCommentsAction);

			// then
			actualCommentServiceException.Should()
				.BeEquivalentTo(expectedCommentServiceException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAllComments(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedCommentServiceException))),
						Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}