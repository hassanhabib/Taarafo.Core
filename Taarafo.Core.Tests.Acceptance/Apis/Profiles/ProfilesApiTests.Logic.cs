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
        public async Task ShouldPostProfileAsync()
        {
            //given
            Profile randomProfile = CreateRandomProfile();
            Profile inputProfile = randomProfile;
            Profile expectedProfile = inputProfile;

            //when
            await this.apiBroker.PostProfilesAsync(inputProfile);

            Profile actualProfile =
                await this.apiBroker.GetProfileByIdAsync(inputProfile.Id);

            //then
            actualProfile.Should().BeEquivalentTo(expectedProfile);
            await this.apiBroker.DeleteProfileByIdAsync(actualProfile.Id);
        }

        [Fact]
        public async Task ShouldGetProfileByIdAsync()
        {
            //given
            Profile randomProfile = await PostRandomProfileAsync();
            Profile expectedProfile = randomProfile;

            //when
            Profile actualProfile = await this.apiBroker.GetProfileByIdAsync(randomProfile.Id);

            //then
            actualProfile.Should().BeEquivalentTo(expectedProfile);
            await this.apiBroker.DeleteProfileByIdAsync(actualProfile.Id);
        }
    }
}
