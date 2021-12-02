// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.Comments.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedCommentStorageException(sqlException);

            var expectedCommentDependencyException =
                new CommentDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllComments())
                    .Throws(sqlException);

            // when
            Action retrieveAllCommentsAction = () =>
                this.commentService.RetrieveAllComments();

            // then
            Assert.Throws<CommentDependencyException>(
                retrieveAllCommentsAction);

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
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedCommentServiceException =
                new FailedCommentServiceException(serviceException);

            var expectedCommentServiceException =
                new CommentServiceException(failedCommentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllComments())
                    .Throws(serviceException);

            // when
            Action retrieveAllCommentsAction = () =>
                this.commentService.RetrieveAllComments();

            // then
            Assert.Throws<CommentServiceException>(
                retrieveAllCommentsAction);

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