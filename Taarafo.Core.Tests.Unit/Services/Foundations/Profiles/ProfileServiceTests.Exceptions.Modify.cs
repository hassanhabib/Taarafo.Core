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
		public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
		{
			// given
			Profile randomProfile = CreateRandomProfile();
			SqlException sqlException = GetSqlException();

			var failedProfileStorageException =
				new FailedProfileStorageException(sqlException);

			var expectedProfileDependencyException =
				new ProfileDependencyException(failedProfileStorageException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(sqlException);

			// when
			ValueTask<Profile> modifyProfileTask =
				this.profileService.ModifyProfileAsync(randomProfile);

			ProfileDependencyException actualProfileDependencyException =
				await Assert.ThrowsAsync<ProfileDependencyException>(
					modifyProfileTask.AsTask);

			// then
			actualProfileDependencyException.Should().BeEquivalentTo(
				expectedProfileDependencyException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectProfileByIdAsync(randomProfile.Id),
					Times.Never);

			this.storageBrokerMock.Verify(broker =>
				broker.UpdateProfileAsync(randomProfile),
					Times.Never);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(
					expectedProfileDependencyException))),
						Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
		{
			// given
			Profile someProfile =
				CreateRandomProfile();

			Profile foreignKeyConflictedProfile =
				someProfile;

			string randomMessage =
				GetRandomMessage();

			string exceptionMessage =
				randomMessage;

			var foreignKeyConstraintConflictException =
				new ForeignKeyConstraintConflictException(exceptionMessage);

			var invalidProfileReferenceException =
				new InvalidProfileReferenceException(foreignKeyConstraintConflictException);

			var profileDependencyValidationException =
				new ProfileDependencyValidationException(invalidProfileReferenceException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(foreignKeyConstraintConflictException);

			// when
			ValueTask<Profile> modifyProfileTask =
				this.profileService.ModifyProfileAsync(foreignKeyConflictedProfile);

			ProfileDependencyValidationException actualProfileDependencyValidationException =
				await Assert.ThrowsAsync<ProfileDependencyValidationException>(
					modifyProfileTask.AsTask);

			// then
			actualProfileDependencyValidationException.Should().BeEquivalentTo(
				profileDependencyValidationException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectProfileByIdAsync(foreignKeyConflictedProfile.Id),
					Times.Never);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(profileDependencyValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.UpdateProfileAsync(foreignKeyConflictedProfile),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
		{
			// given
			Profile randomProfile = CreateRandomProfile();
			var databaseUpdateException = new DbUpdateException();

			var failedProfileStorageException =
				new FailedProfileStorageException(databaseUpdateException);

			var expectedProfileDependencyException =
				new ProfileDependencyException(failedProfileStorageException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(databaseUpdateException);

			// when
			ValueTask<Profile> modifyProfileTask =
				this.profileService.ModifyProfileAsync(randomProfile);

			ProfileDependencyException actualProfileDependencyException =
				await Assert.ThrowsAsync<ProfileDependencyException>(
					modifyProfileTask.AsTask);

			// then
			actualProfileDependencyException.Should().BeEquivalentTo(
				expectedProfileDependencyException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectProfileByIdAsync(randomProfile.Id),
					Times.Never);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedProfileDependencyException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.UpdateProfileAsync(randomProfile),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
		{
			// given
			Profile randomProfile = CreateRandomProfile();
			var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

			var lockedProfileException =
				new LockedProfileException(databaseUpdateConcurrencyException);

			var expectedProfileDependencyValidationException =
				new ProfileDependencyValidationException(lockedProfileException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(databaseUpdateConcurrencyException);

			// when
			ValueTask<Profile> modifyProfileTask =
				this.profileService.ModifyProfileAsync(randomProfile);

			ProfileDependencyValidationException actualProfileDependencyValidationException =
				await Assert.ThrowsAsync<ProfileDependencyValidationException>(
					modifyProfileTask.AsTask);

			// then
			actualProfileDependencyValidationException.Should().BeEquivalentTo(
				expectedProfileDependencyValidationException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectProfileByIdAsync(randomProfile.Id),
					Times.Never);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedProfileDependencyValidationException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.UpdateProfileAsync(randomProfile),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
		{
			// given
			Profile randomProfile = CreateRandomProfile();
			var serviceException = new Exception();

			var failedProfileException =
				new FailedProfileServiceException(serviceException);

			var expectedProfileServiceException =
				new ProfileServiceException(failedProfileException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(serviceException);

			// when
			ValueTask<Profile> modifyProfileTask =
				this.profileService.ModifyProfileAsync(randomProfile);

			ProfileServiceException actualProfileServiceException =
				await Assert.ThrowsAsync<ProfileServiceException>(
					modifyProfileTask.AsTask);

			// then
			actualProfileServiceException.Should().BeEquivalentTo(
				expectedProfileServiceException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectProfileByIdAsync(randomProfile.Id),
					Times.Never);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedProfileServiceException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.UpdateProfileAsync(randomProfile),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}
