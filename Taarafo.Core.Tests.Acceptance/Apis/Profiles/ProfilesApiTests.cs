// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taarafo.Core.Tests.Acceptance.Brokers;
using Taarafo.Core.Tests.Acceptance.Models.Profiles;
using Tynamix.ObjectFiller;
using Xunit;

namespace Taarafo.Core.Tests.Acceptance.Apis.Profiles
{
    [Collection(nameof(ApiTestCollection))]
    public partial class ProfilesApiTests
    {
        private readonly ApiBroker apiBroker;

        public ProfilesApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        public async ValueTask<Profile> PostRandomProfileAsync()
        {
            Profile randomProfile = CreateRandomProfile();
            await this.apiBroker.PostProfilesAsync(randomProfile);

            return randomProfile;
        }

        private static Profile UpdateRandomProfile(Profile inputProfile)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Profile>();

            filler.Setup()
                .OnProperty(profile => profile.Id).Use(inputProfile.Id)
                .OnProperty(profile => profile.CreatedDate).Use(inputProfile.CreatedDate)
                .OnProperty(profile => profile.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private async ValueTask<List<Profile>> CreateRandomProfilesAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomProfiles = new List<Profile>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomProfiles.Add(await PostRandomProfileAsync());
            }

            return randomProfiles;
        }

        private static Profile CreateRandomProfile() =>
            CreateRandomProfileFiller().Create();

        private int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static Filler<Profile> CreateRandomProfileFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Profile>();

            filler.Setup()
                .OnProperty(profile => profile.CreatedDate).Use(now)
                .OnProperty(profile => profile.UpdatedDate).Use(now);

            return filler;
        }
    }
}