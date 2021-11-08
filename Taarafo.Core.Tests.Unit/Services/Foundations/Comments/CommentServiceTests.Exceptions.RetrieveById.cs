// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Taarafo.Core.Models.Comments;
using Taarafo.Core.Models.Comments.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Comments
{
    public partial class CommentServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedCommentStorageException =
                new FailedCommentStorageException(sqlException);

            var expectedCommentDependencyException =
                new CommentDependencyException(failedCommentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Comment> retrieveCommentByIdTask =
                this.commentService.RetrieveCommentByIdAsync(someId);

            // then
            await Assert.ThrowsAsync<CommentDependencyException>(() =>
                retrieveCommentByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCommentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCommentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
