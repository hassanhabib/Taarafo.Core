// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Taarafo.Core.Models.Profiles;
using Taarafo.Core.Models.Profiles.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Profiles
{
	public partial class ProfileServiceTests
	{
		[Fact]
		private async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
		{
			// given
			DateTimeOffset randomDateTime = GetRandomDateTime();
			Profile someProfile = CreateRandomProfile(randomDateTime);
			SqlException sqlException = GetSqlException();

			var failedProfileStorageException =
				new FailedProfileStorageException(
                    message: "Failed profile storage error occurred, contact support.",
					innerException: sqlException);

			var expectedProfileDependencyException =
				new ProfileDependencyException(
                    message: "Profile dependency error occurred, contact support.",
					innerException: failedProfileStorageException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(sqlException);

			// when
			ValueTask<Profile> addProfileTask =
				this.profileService.AddProfileAsync(someProfile);

			ProfileDependencyException actualProfileDependencyException =
				await Assert.ThrowsAsync<ProfileDependencyException>(
					addProfileTask.AsTask);

			// then
			actualProfileDependencyException.Should().BeEquivalentTo(
				expectedProfileDependencyException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(
					expectedProfileDependencyException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertProfileAsync(It.IsAny<Profile>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		private async Task ShouldThrowDependencyValidationExceptionOnAddIfProfileAlreadyExsitsAndLogItAsync()
		{
			// given
			Profile randomProfile = CreateRandomProfile();
			Profile alreadyExistsProfile = randomProfile;
			string randomMessage = GetRandomMessage();

			var duplicateKeyException =
				new DuplicateKeyException(randomMessage);

			var alreadyExistsProfileException =
				new AlreadyExistsProfileException(
                    message: "Profile with the same id already exists.",
					innerException: duplicateKeyException);

			var expectedProfileDependencyValidationException =
				new ProfileDependencyValidationException(
                    message: "Profile dependency validation occurred, please try again.",
					innerException: alreadyExistsProfileException);

			this.dateTimeBrokerMock.Setup(broker =>
			  broker.GetCurrentDateTimeOffset())
				  .Throws(duplicateKeyException);

			// when
			ValueTask<Profile> addProfileTask =
				this.profileService.AddProfileAsync(alreadyExistsProfile);

			ProfileDependencyValidationException actualProfileDependencyValidationException =
				await Assert.ThrowsAsync<ProfileDependencyValidationException>(
					addProfileTask.AsTask);

			// then
			actualProfileDependencyValidationException.Should().BeEquivalentTo(
				expectedProfileDependencyValidationException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedProfileDependencyValidationException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertProfileAsync(It.IsAny<Profile>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		private async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
		{
			// given
			Profile someProfile = CreateRandomProfile();
			string randomMessage = GetRandomMessage();
			string exceptionMessage = randomMessage;

			var foreignKeyConstraintConflictException =
				new ForeignKeyConstraintConflictException(exceptionMessage);

			var invalidProfileReferenceException =
				new InvalidProfileReferenceException(
                    message: "Invalid profile reference error occurred.",
					innerException: foreignKeyConstraintConflictException);

			var expectedProfileDependencyValidationException =
				new ProfileDependencyValidationException(
                    message: "Profile dependency validation occurred, please try again.",
					innerException: invalidProfileReferenceException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(foreignKeyConstraintConflictException);

			// when
			ValueTask<Profile> addProfileTask =
				this.profileService.AddProfileAsync(someProfile);

			ProfileDependencyValidationException actualProfileDependencyValidationException =
				await Assert.ThrowsAsync<ProfileDependencyValidationException>(
					addProfileTask.AsTask);

			// then
			actualProfileDependencyValidationException.Should().BeEquivalentTo(
				expectedProfileDependencyValidationException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedProfileDependencyValidationException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertProfileAsync(It.IsAny<Profile>()),
						Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		private async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
		{
			// given
			Profile someProfile = CreateRandomProfile();

			var databaseUpdateException =
				new DbUpdateException();

			var failedProfileStorageException =
				new FailedProfileStorageException(
                    message: "Failed profile storage error occurred, contact support.",
					innerException: databaseUpdateException);

			var expectedProfileDependencyException =
				new ProfileDependencyException(
                    message: "Profile dependency error occurred, contact support.",
                innerException: failedProfileStorageException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(databaseUpdateException);

			// when
			ValueTask<Profile> addProfileTask =
				this.profileService.AddProfileAsync(someProfile);

			ProfileDependencyException actualProfileDependencyException =
				await Assert.ThrowsAsync<ProfileDependencyException>(
					addProfileTask.AsTask);

			// then
			actualProfileDependencyException.Should().BeEquivalentTo(
				expectedProfileDependencyException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedProfileDependencyException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertProfileAsync(It.IsAny<Profile>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		private async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
		{
			// given
			Profile someProfile = CreateRandomProfile();
			var serviceException = new Exception();

			var failedProfileServiceException =
				new FailedProfileServiceException(
                    message: "Failed profile service occurred, please contact support",
					innerException: serviceException);

			var expectedProfileServiceException =
				new ProfileServiceException(
                    message: "Profile service error occurred, contact support.",
					innerException: failedProfileServiceException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(serviceException);

			// when
			ValueTask<Profile> addProfileTask =
				this.profileService.AddProfileAsync(someProfile);

			ProfileServiceException actualProfileServiceException =
				await Assert.ThrowsAsync<ProfileServiceException>(
					addProfileTask.AsTask);

			// then
			actualProfileServiceException.Should().BeEquivalentTo(
				expectedProfileServiceException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedProfileServiceException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertProfileAsync(It.IsAny<Profile>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}