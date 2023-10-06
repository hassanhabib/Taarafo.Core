// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
		public async Task ShouldThrowCriticalDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
		{
			// given
			Guid someProfileId = Guid.NewGuid();
			SqlException sqlException = GetSqlException();

			var failedProfileStorageException =
				new FailedProfileStorageException(sqlException);

			var expectedProfileDependencyException =
				new ProfileDependencyException(failedProfileStorageException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectProfileByIdAsync(It.IsAny<Guid>()))
					.ThrowsAsync(sqlException);

			// when
			ValueTask<Profile> removeProfileByIdTask =
				this.profileService.RemoveProfileByIdAsync(someProfileId);

			ProfileDependencyException actualProfileDependencyException =
				await Assert.ThrowsAsync<ProfileDependencyException>(
					removeProfileByIdTask.AsTask);

			// then
			actualProfileDependencyException.Should().BeEquivalentTo(
				expectedProfileDependencyException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectProfileByIdAsync(It.IsAny<Guid>()),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(
					expectedProfileDependencyException))),
						Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
		{
			// given
			Guid someProfileId = Guid.NewGuid();

			var databaseUpdateConcurrencyException =
				new DbUpdateConcurrencyException();

			var lockedProfileException =
				new LockedProfileException(databaseUpdateConcurrencyException);

			var expectedProfileDependencyValidationException =
				new ProfileDependencyValidationException(lockedProfileException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectProfileByIdAsync(It.IsAny<Guid>()))
					.ThrowsAsync(databaseUpdateConcurrencyException);

			// when
			ValueTask<Profile> removeProfileByIdTask =
				this.profileService.RemoveProfileByIdAsync(someProfileId);

			ProfileDependencyValidationException actualProfileDependencyValidationException =
				await Assert.ThrowsAsync<ProfileDependencyValidationException>(
					removeProfileByIdTask.AsTask);

			// then
			actualProfileDependencyValidationException.Should().BeEquivalentTo(
				expectedProfileDependencyValidationException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectProfileByIdAsync(It.IsAny<Guid>()),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedProfileDependencyValidationException))),
						Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
		{
			// given
			Guid someProfileId = Guid.NewGuid();
			var serviceException = new Exception();

			var failedProfileServiceException =
				new FailedProfileServiceException(serviceException);

			var expectedProfileServiceException =
				new ProfileServiceException(failedProfileServiceException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectProfileByIdAsync(It.IsAny<Guid>()))
					.ThrowsAsync(serviceException);

			// when
			ValueTask<Profile> removeProfileByIdTask =
				this.profileService.RemoveProfileByIdAsync(someProfileId);

			ProfileServiceException actualProfileServiceException =
				await Assert.ThrowsAsync<ProfileServiceException>(
					removeProfileByIdTask.AsTask);

			// then
			actualProfileServiceException.Should()
				.BeEquivalentTo(expectedProfileServiceException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectProfileByIdAsync(It.IsAny<Guid>()),
						Times.Once());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedProfileServiceException))),
						Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
	}
}
