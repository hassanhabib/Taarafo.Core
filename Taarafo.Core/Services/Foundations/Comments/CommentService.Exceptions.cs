// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Taarafo.Core.Models.Comments;
using Taarafo.Core.Models.Comments.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Foundations.Comments
{
    public partial class CommentService
    {
        private delegate ValueTask<Comment> ReturningCommentFunction();

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
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCommentException =
                    new AlreadyExistsCommentException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsCommentException);
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

        private CommentDependencyValidationException CreateAndLogDependencyValidationException(
        Xeption exception)
        {
            var commentDependencyValidationException =
                new CommentDependencyValidationException(exception);

            this.loggingBroker.LogError(commentDependencyValidationException);

            return commentDependencyValidationException;
        }

        private CommentDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var commentDependencyException = new CommentDependencyException(exception);
            this.loggingBroker.LogCritical(commentDependencyException);

            return commentDependencyException;
        }
    }
}
