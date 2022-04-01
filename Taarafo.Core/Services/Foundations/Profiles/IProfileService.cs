// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Profiles;

namespace Taarafo.Core.Services.Foundations.Profiles
{
    public partial interface IProfileService
    {
        ValueTask<Profile> AddProfileAsync(Profile profile);
        ValueTask<Profile> RetrieveProfileByIdAsync(Guid profileId);
        IQueryable<Profile> RetrieveAllProfiles();
        ValueTask<Profile> ModifyProfileAsync(Profile profile);
        ValueTask<Profile> RemoveProfileByIdAsync(Guid profileId);

    }
}
