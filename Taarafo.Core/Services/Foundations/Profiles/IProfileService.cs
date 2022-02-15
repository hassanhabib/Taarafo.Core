// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Profiles;

namespace Taarafo.Core.Services.Foundations.Profiles
{
    public partial interface IProfileService
    {
        ValueTask<Profile> AddProfileAsync(Profile profile);
        IQueryable<Profile> RetrieveAllProfiles();
        ValueTask<Profile> RetrieveProfileByIdAsync();
    }
}
