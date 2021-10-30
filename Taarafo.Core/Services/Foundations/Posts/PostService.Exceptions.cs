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
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Foundations.Posts
{
    public partial class PostService
    {
        private delegate IQueryable<Post> ReturningPostsFunction();
        private delegate ValueTask<Post> ReturningPostFunction();

        private async ValueTask<Post> TryCatch(ReturningPostFunction returningPostFunction)
        {
            try
            {
                return await returningPostFunction();
            }
            catch (NullPostException nullPostException)
            {
                throw CreateAndLogValidationException(nullPostException);
            }
            catch (InvalidPostException invalidPostException)
            {
                throw CreateAndLogValidationException(invalidPostException);
            }
            catch (SqlException sqlException)
            {
                var failedPostStorageException =
                    new FailedPostStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedPostStorageException);
            }
            catch (NotFoundPostException notFoundPostException)
            {
                throw CreateAndLogValidationException(notFoundPostException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsPostException =
                    new AlreadyExistsPostException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsPostException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedPostException = new LockedPostException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedPostException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedPostStorageException =
                    new FailedPostStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedPostStorageException);
            }
            catch (Exception exception)
            {
                var failedPostServiceException =
                    new FailedPostServiceException(exception);

                throw CreateAndLogServiceException(failedPostServiceException);
            }
        }

        private IQueryable<Post> TryCatch(ReturningPostsFunction returningPostsFunction)
        {
            try
            {
                return returningPostsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedPostStorageException =
                    new FailedPostStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedPostStorageException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private PostValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var postValidationException =
                new PostValidationException(exception);

            this.loggingBroker.LogError(postValidationException);

            return postValidationException;
        }

        private PostDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var postDependencyException = new PostDependencyException(exception);
            this.loggingBroker.LogCritical(postDependencyException);

            return postDependencyException;
        }

        private PostDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var postDependencyValidationException =
                new PostDependencyValidationException(exception);

            this.loggingBroker.LogError(postDependencyValidationException);

            return postDependencyValidationException;
        }

        private PostDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var postDependencyException = new PostDependencyException(exception);
            this.loggingBroker.LogError(postDependencyException);

            return postDependencyException;
        }

        private PostServiceException CreateAndLogServiceException(
            Exception exception)
        {
            var postServiceException = new PostServiceException(exception);
            this.loggingBroker.LogError(postServiceException);

            return postServiceException;
        }
    }
}
