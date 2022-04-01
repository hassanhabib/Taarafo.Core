// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Brokers.DateTimes;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Profiles;

namespace Taarafo.Core.Services.Foundations.Profiles
{
    public partial class ProfileService : IProfileService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public ProfileService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Profile> AddProfileAsync(Profile profile) =>
        TryCatch(async () =>
        {
            ValidateProfileOnAdd(profile);

            return await this.storageBroker.InsertProfileAsync(profile);
        });

        public ValueTask<Profile> RetrieveProfileByIdAsync(Guid profileId) =>
        TryCatch(async () =>
        {
            ValidateProfileId(profileId);

            var maybeProfile =
                await this.storageBroker.SelectProfileByIdAsync(profileId);

            ValidateStorageProfile(maybeProfile, profileId);

            return maybeProfile;
        });

        public IQueryable<Profile> RetrieveAllProfiles() =>
        TryCatch(() =>
        {
            return this.storageBroker.SelectAllProfiles();
        });

        public ValueTask<Profile> ModifyProfileAsync(Profile profile) =>
        TryCatch(async () =>
        {
            ValidateProfileOnModify(profile);

            var maybeProfile =
                await this.storageBroker.SelectProfileByIdAsync(profile.Id);

            ValidateStorageProfile(maybeProfile, profile.Id);

            return
                await this.storageBroker.UpdateProfileAsync(profile);
        });

        public ValueTask<Profile> RemoveProfileByIdAsync(Guid profileId) =>
        TryCatch(async () =>
        {
            ValidateProfileId(profileId);

            Profile someProfile =
                await this.storageBroker.SelectProfileByIdAsync(profileId);

            return await this.storageBroker.DeleteProfileAsync(someProfile);
        });
    }
}
