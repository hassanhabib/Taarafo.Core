// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Taarafo.Core.Models.Posts.Exceptions;
using Taarafo.Core.Models.Profiles;
using Taarafo.Core.Models.Profiles.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Foundations.Profiles
{
    public partial class ProfileService
    {
        private delegate ValueTask<Profile> ReturningProfileFunction();

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
            catch(SqlException sqlException)
            {
                var failedProfileStorageException =
                    new FailedProfileStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProfileStorageException);
            }
            catch(DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistProfileException =
                    new AlreadyExistsProfileException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistProfileException);
            }
            catch (InvalidProfileException invalidProfileException)
            {
                throw CreateAndLogValidationException(invalidProfileException);
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

        private ProfileDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var profileDependencyValidationException =
                new ProfileDependencyValidationException(exception);

            this.loggingBroker.LogError(profileDependencyValidationException);

            return profileDependencyValidationException;
        }
    }
}
