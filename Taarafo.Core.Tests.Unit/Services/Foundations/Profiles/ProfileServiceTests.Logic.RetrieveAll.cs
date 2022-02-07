// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.Profiles;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Profiles
{
    public partial class ProfileServiceTests
    {
        [Fact]
        public void ShouldRetrievAllProfiles()
        {
            // given
            IQueryable<Profile> randomProfiles = CreateRandomProfiles();
            IQueryable<Profile> storageProfiles = randomProfiles;
            IQueryable<Profile> expectedProfiles = storageProfiles;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProfiles())
                    .Returns(storageProfiles);

            // when
            IQueryable<Profile> actualProfiles =
                this.profileService.RetrieveAllProfiles();

            // then
            actualProfiles.Should().BeEquivalentTo(expectedProfiles);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProfiles(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
