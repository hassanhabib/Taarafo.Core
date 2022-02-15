// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Taarafo.Core.Models.Profiles;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Profile> Profiles { get; set; }

        public async ValueTask<Profile> InsertProfileAsync(Profile profile)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Profile> profileEntityEntry =
                await broker.Profiles.AddAsync(profile);

            await broker.SaveChangesAsync();

            return profileEntityEntry.Entity;
        }

        public async ValueTask<Profile> UpdateProfileAsync(Profile profile)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Profile> profileEntityEntry =
                broker.Profiles.Update(profile);

            await broker.SaveChangesAsync();

            return profileEntityEntry.Entity;
        }
    }
}
