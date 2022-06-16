// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Foundations.PostImpressions
{
    public partial class PostImpressionService
    {
        private delegate ValueTask<PostImpression> ReturningPostImpressionFunction();

        private async ValueTask<PostImpression> TryCatch(ReturningPostImpressionFunction returningPostImpressionFunction)
        {
            try
            {
                return await returningPostImpressionFunction();
            }
            catch (NullPostImpressionException nullPostImpressionException)
            {
                throw CreateAndLogValidationException(nullPostImpressionException);
            }
            catch (InvalidPostImpressionException invalidPostImpressionException)
            {
                throw CreateAndLogValidationException(invalidPostImpressionException);
            }
            catch (SqlException sqlException)
            {
                var failedPostImpressionStorageException =
                    new FailedPostImpressionStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedPostImpressionStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsPostImpressionException =
                    new AlreadyExistsPostImpressionException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsPostImpressionException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedPostImpressionStorageException =
                    new FailedPostImpressionStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedPostImpressionStorageException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidPostImpressionReferenceException =
                    new InvalidPostImpressionReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidPostImpressionReferenceException);
            }
        }

        private PostImpressionValidationException CreateAndLogValidationException(Xeption exception)
        {
            var postImpressionValidationException =
                new PostImpressionValidationException(exception);

            this.loggingBroker.LogError(postImpressionValidationException);

            return postImpressionValidationException;
        }

        private PostImpressionDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var postImpressionDependencyException = new PostImpressionDependencyException(exception);
            this.loggingBroker.LogCritical(postImpressionDependencyException);

            return postImpressionDependencyException;
        }

        private PostImpressionDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var postImpressionDependencyValidationException =
                new PostImpressionDependencyValidationException(exception);

            this.loggingBroker.LogError(postImpressionDependencyValidationException);

            return postImpressionDependencyValidationException;
        }

        private PostImpressionDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var postImpressionDependencyException = new PostImpressionDependencyException(exception);
            this.loggingBroker.LogError(postImpressionDependencyException);

            return postImpressionDependencyException;
        }
    }
}
