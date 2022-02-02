// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Taarafo.Core.Models.Profiles;
using Taarafo.Core.Models.Profiles.Exceptions;

namespace Taarafo.Core.Services.Foundations.Profiles
{
    public partial class ProfileService
    {
        private void ValidateProfileOnAdd(Profile profile)
        {
            ValidateProfileIsNotNull(profile);
        }

        private void ValidateProfileIsNotNull(Profile profile)
        {
            if (profile is null)
            {
                throw new NullProfileException();
            }
        }
    }
}
