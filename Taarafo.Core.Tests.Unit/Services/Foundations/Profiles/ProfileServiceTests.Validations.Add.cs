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
        public async Task ShouldThrowValidationExceptionOnAddIfProfileIsNullAndLogItAsync()
        {
            // given
            Profile invalidProfile = null;

            var nullProfileException =
                new NullProfileException();

            var expectedProfileValidationException =
                new ProfileValidationException(nullProfileException);

            // when
            ValueTask<Profile> addProfileTask =
                this.profileService.AddProfileAsync(invalidProfile);

            // then
            await Assert.ThrowsAsync<ProfileValidationException>(() =>
                addProfileTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProfileValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProfileAsync(invalidProfile),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfProfileIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidProfile = new Profile
            {
                Username = invalidText,
                Email = invalidText
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
                values: "Date is required");

            var expectedProfileValidationException =
                new ProfileValidationException(invalidProfileException);

            // when
            ValueTask<Profile> addProfileTask =
                this.profileService.AddProfileAsync(invalidProfile);

            // then
            await Assert.ThrowsAsync<ProfileValidationException>(() =>
                addProfileTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProfileValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProfileAsync(invalidProfile),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            int randomNumber = GetRandomNumber();
            Profile randomProfile = CreateRandomProfile(randomDateTime);
            Profile invalidProfile = randomProfile;

            invalidProfile.UpdatedDate =
                invalidProfile.CreatedDate.AddDays(randomNumber);

            var invalidProfileException =
                new InvalidProfileException();

            invalidProfileException.AddData(
                key: nameof(Profile.UpdatedDate),
                values: $"Date is not the same as {nameof(Profile.CreatedDate)}");

            var expectedProfileValidationException =
                new ProfileValidationException(invalidProfileException);

            this.dateTimeBrokerMock.Setup(broker =>
              broker.GetCurrentDateTimeOffset())
                  .Returns(randomDateTime);

            // when
            ValueTask<Profile> addProfileTask =
                this.profileService.AddProfileAsync(invalidProfile);

            ProfileValidationException  actualProfileValidationException =
                await Assert.ThrowsAsync<ProfileValidationException>(() =>
                    addProfileTask.AsTask());

            // then
            actualProfileValidationException.Should().BeEquivalentTo(expectedProfileValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProfileValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProfileAsync(It.IsAny<Profile>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTime =
                GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTime.AddMinutes(minutesBeforeOrAfter);

            Profile randomProfile = CreateRandomProfile(invalidDateTime);
            Profile invalidProfile = randomProfile;

            var invalidProfileException =
                new InvalidProfileException();

            invalidProfileException.AddData(
                key: nameof(Profile.CreatedDate),
                values: "Date is not recent");

            var expectedProfileValidationException =
                new ProfileValidationException(invalidProfileException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Profile> addProfileTask =
                this.profileService.AddProfileAsync(invalidProfile);

            // then
            await Assert.ThrowsAsync<ProfileValidationException>(() =>
               addProfileTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProfileValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProfileAsync(It.IsAny<Profile>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
