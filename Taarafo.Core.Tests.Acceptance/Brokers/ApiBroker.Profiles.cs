// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taarafo.Core.Tests.Acceptance.Models.Profiles;

namespace Taarafo.Core.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string ProfilesRelativeUrl = "api/profiles";

        public async ValueTask<Profile> PostProfilesAsync(Profile profile) =>
            await this.apiFactoryClient.PostContentAsync(ProfilesRelativeUrl, profile);

        public async ValueTask<List<Profile>> GetAllProfilesAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Profile>>($"{ProfilesRelativeUrl}/");

        public async ValueTask<Profile> GetProfileByIdAsync(Guid profileId) =>
            await this.apiFactoryClient.GetContentAsync<Profile>($"{ProfilesRelativeUrl}/{profileId}");

        public async ValueTask<Profile> DeleteProfileByIdAsync(Guid profileId) =>
            await this.apiFactoryClient.DeleteContentAsync<Profile>($"{ProfilesRelativeUrl}/{profileId}");
    }
}
