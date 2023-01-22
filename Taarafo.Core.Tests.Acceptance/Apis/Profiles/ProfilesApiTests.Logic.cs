// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Taarafo.Core.Tests.Acceptance.Models.Profiles;
using Xunit;

namespace Taarafo.Core.Tests.Acceptance.Apis.Profiles
{
    public partial class ProfilesApiTests
    {
        [Fact]
        public async Task ShouldGetAllProfileAsync()
        {
            //given
            List<Profile> randomProfiles = await CreateRandomProfilesAsync();
            List<Profile> expectedProfiles = randomProfiles;

            //when
            List<Profile> actualProfiles = await this.apiBroker.GetAllProfilesAsync();

            //then
            foreach (Profile expectedProfile in expectedProfiles)
            {
                Profile actualProfile = actualProfiles.Single(profile => profile.Id == expectedProfile.Id);
                actualProfile.Should().BeEquivalentTo(expectedProfile);
                await this.apiBroker.DeleteProfileByIdAsync(actualProfile.Id);
            }
        }
    }
}
