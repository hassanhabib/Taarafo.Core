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
using Taarafo.Core.Models.Profiles;
using Taarafo.Core.Models.Profiles.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Foundations.Profiles
{
    public partial class ProfileService
    {
        private delegate ValueTask<Profile> ReturningProfileFunction();
        private delegate IQueryable<Profile> ReturningProfilesFunction();

        private async ValueTask<Profile> TryCatch(ReturningProfileFunction returningProfileFunction)
        {
            try
            {
                return await returningProfileFunction();
            }
            catch (NullProfileException nullProfileException)
            {
                throw CreateAndLogValidationException(nullProfileException);
            }
            catch (InvalidProfileException invalidProfileException)
            {
                throw CreateAndLogValidationException(invalidProfileException);
            }
            catch (NotFoundProfileException notFoundProfileException)
            {
                throw CreateAndLogValidationException(notFoundProfileException);
            }
            catch (SqlException sqlException)
            {
                var failedProfileStorageException =
                    new FailedProfileStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProfileStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistProfileException =
                    new AlreadyExistsProfileException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistProfileException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidProfileReferenceException =
                    new InvalidProfileReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidProfileReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedStorageProfileException =
                    new FailedProfileStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedStorageProfileException);
            }
            catch (Exception serviceException)
            {
                var failedServiceProfileException =
                    new FailedProfileServiceException(serviceException);

                throw CreateAndLogServiceException(failedServiceProfileException);
            }
        }

        private IQueryable<Profile> TryCatch(ReturningProfilesFunction returningProfilesFunction)
        {
            try
            {
                return returningProfilesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedProfileStorageException =
                    new FailedProfileStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProfileStorageException);
            }
            catch (Exception serviceException)
            {
                var failedProfileServiceException =
                    new FailedProfileServiceException(serviceException);

                throw CreateAndLogServiceException(failedProfileServiceException);
            }
        }

        private ProfileValidationException CreateAndLogValidationException(Xeption exception)
        {
            var profileValidationException =
                new ProfileValidationException(exception);

            this.loggingBroker.LogError(profileValidationException);

            return profileValidationException;
        }

        private ProfileDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var profileDependencyException =
                new ProfileDependencyException(exception);

            this.loggingBroker.LogCritical(profileDependencyException);

            return profileDependencyException;
        }

        private ProfileDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var profileDependencyException =
                new ProfileDependencyException(exception);

            this.loggingBroker.LogError(profileDependencyException);

            return profileDependencyException;
        }

        private ProfileDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var profileDependencyValidationException =
                new ProfileDependencyValidationException(exception);

            this.loggingBroker.LogError(profileDependencyValidationException);

            return profileDependencyValidationException;
        }

        private ProfileServiceException CreateAndLogServiceException(Xeption exception)
        {
            var profileServiceException =
                new ProfileServiceException(exception);

            this.loggingBroker.LogError(profileServiceException);

            return profileServiceException;
        }
    }
}
