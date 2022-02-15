// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Taarafo.Core.Models.Profiles;

namespace Taarafo.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Profile> InsertProfileAsync(Profile profile);
        ValueTask<Profile> UpdateProfileAsync(Profile profile);
    }
}
