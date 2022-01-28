// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
        public async Task ShouldAddProfileAsync()
        {
            // given
            Profile randomProfile = CreateRandomProfile();
            Profile inputProfile = randomProfile;
            Profile insertedProfile = inputProfile;
            Profile expectedProfile = insertedProfile.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProfileAsync(inputProfile))
                    .ReturnsAsync(insertedProfile);

            // when
            Profile actualProfile =
                await this.profileService.AddProfileAsync(inputProfile);

            // then
            actualProfile.Should().BeEquivalentTo(expectedProfile);

            this.storageBrokerMock.Verify(broker => 
                broker.InsertProfileAsync(inputProfile), 
                    Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
