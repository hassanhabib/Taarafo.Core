﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Taarafo.Core.Models.Profiles;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Profile> Profiles { get; set; }

        public async ValueTask<Profile> InsertProfileAsync(Profile profile) =>
            await InsertAsync(profile);

        public IQueryable<Profile> SelectAllProfiles() =>
            SelectAll<Profile>();

        public async ValueTask<Profile> SelectProfileByIdAsync(Guid profileId) =>
            await SelectAsync<Profile>(profileId);

        public async ValueTask<Profile> UpdateProfileAsync(Profile profile) =>
            await UpdateAsync(profile);

        public async ValueTask<Profile> DeleteProfileAsync(Profile profile) =>
            await DeleteAsync(profile);
    }
}
