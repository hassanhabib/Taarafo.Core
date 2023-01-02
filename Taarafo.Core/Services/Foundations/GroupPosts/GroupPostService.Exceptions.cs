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
using Taarafo.Core.Models.GroupPosts;
using Taarafo.Core.Models.GroupPosts.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Foundations.GroupPosts
{
    public partial class GroupPostService
    {
        private delegate ValueTask<GroupPost> ReturningPostFuntion();
        private delegate IQueryable<GroupPost> ReturningGroupPostsFunction();

        private async ValueTask<GroupPost> TryCatch(ReturningPostFuntion returningPostFuntion)
        {
            try
            {
                return await returningPostFuntion();
            }
            catch (NullGroupPostException nullGroupPostException)
            {
                throw CreateAndLogValidationException(nullGroupPostException);
            }
            catch (InvalidGroupPostException invalidGroupPostException)
            {
                throw CreateAndLogValidationException(invalidGroupPostException);
            }
            catch (NotFoundGroupPostException notFoundGroupPostException)
            {
                throw CreateAndLogValidationException(notFoundGroupPostException);
            }
            catch (SqlException sqlException)
            {
                var failedGroupPostStorageException =
                    new FailedGroupPostStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(
                    failedGroupPostStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsGroupPostException =
                    new AlreadyExistsGroupPostException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(
                    alreadyExistsGroupPostException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedGroupPostException =
                    new LockedGroupPostException(dbUpdateConcurrencyException);

                throw CreateAndDependencyValidationException(lockedGroupPostException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedGroupPostStorageException =
                    new FailedGroupPostStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedGroupPostStorageException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidGroupPostReferenceException =
                    new InvalidGroupPostReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidGroupPostReferenceException);
            }
            catch (Exception exception)
            {
                var failedGroupPostServiceException =
                    new FailedGroupPostServiceException(exception);

                throw CreateAndLogServiceException(failedGroupPostServiceException);
            }
        }

        private IQueryable<GroupPost> TryCatch(ReturningGroupPostsFunction returningGroupPostsFunction)
        {
            try
            {
                return returningGroupPostsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedGroupPostStorageException = new FailedGroupPostStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedGroupPostStorageException);
            }
            catch (Exception serviceException)
            {
                var failedGroupPostServiceException = new FailedGroupPostServiceException(serviceException);

                throw CreateAndLogServiceException(failedGroupPostServiceException);
            }
        }

        private GroupPostValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var groupPostValidationException =
                new GroupPostValidationException(exception);

            this.loggingBroker.LogError(groupPostValidationException);

            return groupPostValidationException;
        }

        private GroupPostDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var groupPostDependencyException =
                new GroupPostDependencyException(exception);

            this.loggingBroker.LogCritical(groupPostDependencyException);

            return groupPostDependencyException;
        }

        private GroupPostDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var groupPostDependencyValidationException =
                new GroupPostDependencyValidationException(exception);

            this.loggingBroker.LogError(groupPostDependencyValidationException);

            return groupPostDependencyValidationException;
        }

        private GroupPostDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var groupPostDependencyException =
                new GroupPostDependencyException(exception);

            this.loggingBroker.LogError(groupPostDependencyException);

            return groupPostDependencyException;
        }

        private GroupPostServiceException CreateAndLogServiceException(Exception exception)
        {
            var groupPostServiceException = new GroupPostServiceException(exception as Xeption);
            this.loggingBroker.LogError(groupPostServiceException);

            return groupPostServiceException;
        }

        private GroupPostDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var groupPostDependencyValidationException = new GroupPostDependencyValidationException(exception);
            this.loggingBroker.LogError(groupPostDependencyValidationException);

            return groupPostDependencyValidationException;
        }
    }
}
