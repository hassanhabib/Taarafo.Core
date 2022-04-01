// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using Taarafo.Core.Models.Profiles;
using Taarafo.Core.Models.Profiles.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Profiles
{
    public partial class ProfileServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidProfileId = Guid.Empty;

            var invalidProfileException =
                new InvalidProfileException();

            invalidProfileException.AddData(
                key: nameof(Profile.Id),
                values: "Id is required");

            var expectedProfileValidationException =
                new ProfileValidationException(invalidProfileException);

            // when
            ValueTask<Profile> removeProfileByIdTask =
                this.profileService.RemoveProfileByIdAsync(invalidProfileId);

            // then
            await Assert.ThrowsAsync<ProfileValidationException>(() =>
                removeProfileByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProfileValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProfileByIdAsync(invalidProfileId),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProfileAsync(It.IsAny<Profile>()),
                    Times.Never);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIdProfileIsNotFounfAndLogItAsync()
        {
            // given
            Guid inputProfileId = Guid.NewGuid();
            Profile noProfile = null;

            var notFoundProfileException =
                new NotFoundProfileException(inputProfileId);

            var expectedProfileValidationException =
                new ProfileValidationException(notFoundProfileException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProfileByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noProfile);

            // when
            ValueTask<Profile> removeProfileByIdTask =
                this.profileService.RemoveProfileByIdAsync(inputProfileId);

            // then
            await Assert.ThrowsAsync<ProfileValidationException>(() =>
                removeProfileByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProfileByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedProfileValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProfileAsync(It.IsAny<Profile>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }
    }
}
