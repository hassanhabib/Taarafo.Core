// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.Comments;
using Taarafo.Core.Models.Comments.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Foundations.Comments
{
    public partial class CommentService
    {
        private delegate ValueTask<Comment> ReturningCommentFunction();
        private delegate IQueryable<Comment> ReturningCommentsFunction();

        private async ValueTask<Comment> TryCatch(ReturningCommentFunction returningCommentFunction)
        {
            try
            {
                return await returningCommentFunction();
            }
            catch (NullCommentException nullCommentException)
            {
                throw CreateAndLogValidationException(nullCommentException);
            }
            catch (InvalidCommentException invalidCommentException)
            {
                throw CreateAndLogValidationException(invalidCommentException);
            }
            catch (SqlException sqlException)
            {
                var failedCommentStorageException =
                    new FailedCommentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedCommentStorageException);
            }
            catch (NotFoundCommentException notFoundCommentException)
            {
                throw CreateAndLogValidationException(notFoundCommentException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCommentException =
                    new AlreadyExistsCommentException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsCommentException);
            }
            catch (ForeignKeyCommentReferenceException foreignKeyCommentReferenceException)
            {
                var invalidCommentReferenceException =
                    new InvalidCommentReferenceException(foreignKeyCommentReferenceException);

                throw CreateAndLogDependencyValidationException(invalidCommentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedCommentException = new LockedCommentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedCommentException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedCommentStorageException =
                    new FailedCommentStorageException(databaseUpdateException);

                throw CreateAndLogDependecyException(failedCommentStorageException);
            }
            catch (Exception exception)
            {
                var failedCommentServiceException =
                    new FailedCommentServiceException(exception);

                throw CreateAndLogServiceException(failedCommentServiceException);
            }
        }

        private IQueryable<Comment> TryCatch(ReturningCommentsFunction returningCommentsFunction)
        {
            try
            {
                return returningCommentsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedCommentStorageException =
                    new FailedCommentStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedCommentStorageException);
            }
            catch (Exception exception)
            {
                var failedCommentServiceException =
                    new FailedCommentServiceException(exception);

                throw CreateAndLogServiceException(failedCommentServiceException);
            }
        }

        private CommentValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var commentValidationException =
                new CommentValidationException(exception);

            this.loggingBroker.LogError(commentValidationException);

            return commentValidationException;
        }

        private CommentDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var commentDependencyException = new CommentDependencyException(exception);
            this.loggingBroker.LogCritical(commentDependencyException);

            return commentDependencyException;
        }

        private CommentDependencyValidationException CreateAndLogDependencyValidationException(
        Xeption exception)
        {
            var commentDependencyValidationException =
                new CommentDependencyValidationException(exception);

            this.loggingBroker.LogError(commentDependencyValidationException);

            return commentDependencyValidationException;
        }

        private CommentDependencyException CreateAndLogDependecyException(
            Xeption exception)
        {
            var commentDependencyException = new CommentDependencyException(exception);
            this.loggingBroker.LogError(commentDependencyException);

            return commentDependencyException;
        }

        private CommentServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var commentServiceException = new CommentServiceException(exception);
            this.loggingBroker.LogError(commentServiceException);

            return commentServiceException;
        }

        private CommentServiceException CreateAndLogServiceException(
            Exception exception)
        {
            var commentServiceException = new CommentServiceException(exception);
            this.loggingBroker.LogError(commentServiceException);

            return commentServiceException;
        }
    }
}
