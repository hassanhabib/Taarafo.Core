﻿// ---------------------------------------------------------------
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

			ProfileValidationException actualProfileValidationException =
				await Assert.ThrowsAsync<ProfileValidationException>(() =>
					removeProfileByIdTask.AsTask());

			// then
			actualProfileValidationException.Should()
				.BeEquivalentTo(expectedProfileValidationException);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedProfileValidationException))),
						Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

		[Fact]
		public async Task ShouldThrowNotFoundExceptionOnRemoveProfileByIdIsNotFounfAndLogItAsync()
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

			ProfileValidationException actualProfileValidationException =
				await Assert.ThrowsAsync<ProfileValidationException>(() =>
					removeProfileByIdTask.AsTask());

			// then
			actualProfileValidationException.Should()
				.BeEquivalentTo(expectedProfileValidationException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectProfileByIdAsync(It.IsAny<Guid>()),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedProfileValidationException))),
						Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
