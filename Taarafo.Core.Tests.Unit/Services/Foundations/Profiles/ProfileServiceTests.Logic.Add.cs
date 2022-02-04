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
        public async Task ShouldAddProfileAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Profile randomProfile = CreateRandomProfile(dateTime);
            Profile inputProfile = randomProfile;
            Profile insertedProfile = inputProfile;
            Profile expectedProfile = insertedProfile.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProfileAsync(inputProfile))
                    .ReturnsAsync(insertedProfile);

            // when
            Profile actualProfile =
                await this.profileService.AddProfileAsync(inputProfile);

            // then
            actualProfile.Should().BeEquivalentTo(expectedProfile);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker => 
                broker.InsertProfileAsync(inputProfile), 
                    Times.Once());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
