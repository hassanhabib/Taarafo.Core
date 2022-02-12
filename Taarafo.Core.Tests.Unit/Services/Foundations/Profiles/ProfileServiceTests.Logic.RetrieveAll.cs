// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
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
        public void ShouldRetrieveProfiles()
        {
            // given
            IQueryable<Profile> randomProfiles = CreatedRandomProfiles();
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
