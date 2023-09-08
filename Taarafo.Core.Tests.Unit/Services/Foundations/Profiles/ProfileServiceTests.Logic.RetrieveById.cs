// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
        private async void ShouldRetrieveProfileByIdAsync()
        {
            // given
            Profile someProfile =
                CreateRandomProfile();

            Profile storageProfile = someProfile;

            Profile expectedProfile =
                storageProfile.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProfileByIdAsync(someProfile.Id))
                    .ReturnsAsync(storageProfile);

            // when
            Profile actualProfile =
                await this.profileService.
                    RetrieveProfileByIdAsync(someProfile.Id);

            // then
            actualProfile.Should().BeEquivalentTo(expectedProfile);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProfileByIdAsync(someProfile.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}