// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        }

        private ProfileValidationException CreateAndLogValidationException(Xeption exception)
        {
            var profileValidationException = 
                new ProfileValidationException(exception);

            this.loggingBroker.LogError(profileValidationException);

            return profileValidationException;
        }
    }
}
