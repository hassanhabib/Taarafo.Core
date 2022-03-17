// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.Profiles;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Profiles
{
    public partial class ProfileServiceTests
    {
        [Fact]
        public async Task ShouldModifyProfileAsync()
        {
            // given
            DateTimeOffset randomDate =
                GetRandomDateTimeOffset();

            Profile randomProfile =
                CreateRandomProfile(randomDate);

            Profile inputProfile =
                randomProfile;

            Profile storageProfile =
                inputProfile.DeepClone();

            Profile updatedProfile =
                inputProfile;

            Profile expectedProfile =
                updatedProfile.DeepClone();

            Guid profileId =
                inputProfile.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProfileByIdAsync(profileId))
                    .ReturnsAsync(storageProfile);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateProfileAsync(inputProfile))
                    .ReturnsAsync(updatedProfile);

            // when
            Profile actualProfile =
                await this.profileService.ModifyProfileAsync(inputProfile);

            // then
            actualProfile.Should().BeEquivalentTo(expectedProfile);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProfileByIdAsync(profileId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProfileAsync(inputProfile),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
