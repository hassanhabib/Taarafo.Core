// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.Profiles;
using Taarafo.Core.Models.Profiles.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Profiles
{
    public partial class ProfileServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfProfileIsNullAndLogItAsync()
        {
            // given
            Profile nullProfile = null;
            var nullProfileException = new NullProfileException();

            var expectedProfileValidationException =
                new ProfileValidationException(nullProfileException);

            // when
            ValueTask<Profile> modifyProfileTask =
                this.profileService.ModifyProfileAsync(nullProfile);

            ProfileValidationException actualProfileValidationException =
                await Assert.ThrowsAsync<ProfileValidationException>(
                modifyProfileTask.AsTask);

            // then
            actualProfileValidationException.Should()
                .BeEquivalentTo(expectedProfileValidationException);
            
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProfileValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProfileByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProfileAsync(It.IsAny<Profile>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfProfileIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidProfile = new Profile
            {
                Username = invalidText
            };

            var invalidProfileException =
                new InvalidProfileException();

            invalidProfileException.AddData(
                key: nameof(Profile.Id),
                values: "Id is required");

            invalidProfileException.AddData(
                key: nameof(Profile.Name),
                values: "Text is required");

            invalidProfileException.AddData(
                key: nameof(Profile.Username),
                values: "Text is required");

            invalidProfileException.AddData(
                key: nameof(Profile.Email),
                values: "Text is required");

            invalidProfileException.AddData(
                key: nameof(Profile.CreatedDate),
                values: "Date is required");

            invalidProfileException.AddData(
                key: nameof(Profile.UpdatedDate),
                    values: new[]
                    {
                        "Date is required",
                        $"Date is the same as {nameof(Profile.CreatedDate)}",
                        "Date is not recent"
                    }
                );

            var expectedProfileValidationException =
                new ProfileValidationException(invalidProfileException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(GetRandomDateTime);

            // when
            ValueTask<Profile> modifyProfileTask =
                this.profileService.ModifyProfileAsync(invalidProfile);

            ProfileValidationException actualProfileValidationException =
                await Assert.ThrowsAsync<ProfileValidationException>(
                    modifyProfileTask.AsTask);

            //then
            actualProfileValidationException.Should()
                .BeEquivalentTo(expectedProfileValidationException);
            
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProfileValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProfileAsync(It.IsAny<Profile>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Profile randomProfile = CreateRandomProfile(randomDateTime);
            Profile invalidProfile = randomProfile;
            var invalidProfileException = new InvalidProfileException();

            invalidProfileException.AddData(
                key: nameof(Profile.UpdatedDate),
                values: $"Date is the same as {nameof(Profile.CreatedDate)}");

            var expectedProfileValidationException =
                new ProfileValidationException(invalidProfileException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Profile> modifyProfileTask =
                this.profileService.ModifyProfileAsync(invalidProfile);

            ProfileValidationException actualProfileValidationException =
                await Assert.ThrowsAsync<ProfileValidationException>(
                    modifyProfileTask.AsTask);

            // then
            actualProfileValidationException.Should()
                .BeEquivalentTo(expectedProfileValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProfileValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProfileByIdAsync(invalidProfile.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            Profile randomProfile = CreateRandomProfile(dateTime);
            Profile inputProfile = randomProfile;
            inputProfile.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidProfileException =
                new InvalidProfileException();

            invalidProfileException.AddData(
                key: nameof(Profile.UpdatedDate),
                values: "Date is not recent");

            var expectedProfileValidatonException =
                new ProfileValidationException(invalidProfileException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(dateTime);

            // when
            ValueTask<Profile> modifyProfileTask =
                this.profileService.ModifyProfileAsync(inputProfile);

            ProfileValidationException actualProfileValidationException =
                await Assert.ThrowsAsync<ProfileValidationException>(
                    modifyProfileTask.AsTask);

            // then
            actualProfileValidationException.Should()
                .BeEquivalentTo(expectedProfileValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProfileValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProfileByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProfileAsync(It.IsAny<Profile>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfProfileDoesNotExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            Profile randomProfile = CreateRandomProfile(dateTime);
            Profile nonExistProfile = randomProfile;
            nonExistProfile.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Profile nullProfile = null;

            var notFoundProfileException =
                new NotFoundProfileException(nonExistProfile.Id);

            var expectedProfileValidationException =
                new ProfileValidationException(notFoundProfileException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProfileByIdAsync(nonExistProfile.Id))
                    .ReturnsAsync(nullProfile);

            // when 
            ValueTask<Profile> modifyProfileTask =
                this.profileService.ModifyProfileAsync(nonExistProfile);

            ProfileValidationException actualProfileValidationException =
                await Assert.ThrowsAsync<ProfileValidationException>(
                    modifyProfileTask.AsTask);

            // then
            actualProfileValidationException.Should()
                .BeEquivalentTo(expectedProfileValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProfileByIdAsync(nonExistProfile.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProfileValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
