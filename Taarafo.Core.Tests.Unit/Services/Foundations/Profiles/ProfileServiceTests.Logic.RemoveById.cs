// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
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
        public async void ShouldRemoveProfileByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputProfileId = randomId;
            Profile randomProfile = CreateRandomProfile();
            Profile storageProfile = randomProfile;
            Profile expectedInputProfile = storageProfile;
            Profile deletedProfile = expectedInputProfile;
            Profile expectedProfile = deletedProfile.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProfileByIdAsync(inputProfileId))
                    .ReturnsAsync(storageProfile);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteProfileAsync(expectedInputProfile))
                    .ReturnsAsync(deletedProfile);

            // when
            Profile actualProfile = await this.profileService
                .RemoveProfileByIdAsync(inputProfileId);

            // then
            actualProfile.Should().BeEquivalentTo(expectedProfile);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProfileByIdAsync(inputProfileId),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProfileAsync(expectedInputProfile),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
