﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.Profiles;
using Taarafo.Core.Models.Profiles.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Profiles
{
	public partial class ProfileServiceTests
	{
		[Fact]
		public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
		{
			// given
			Guid someId = Guid.NewGuid();
			SqlException sqlException = GetSqlException();

			var failedProfileStorageException =
				new FailedProfileStorageException(sqlException);

			var expectedProfileDependencyException =
				new ProfileDependencyException(failedProfileStorageException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectProfileByIdAsync(It.IsAny<Guid>()))
					.ThrowsAsync(sqlException);

			// when
			ValueTask<Profile> retrieveProfileByIdTask =
				this.profileService.RetrieveProfileByIdAsync(someId);

			ProfileDependencyException actaulProfileDependencyException =
				await Assert.ThrowsAsync<ProfileDependencyException>(
					retrieveProfileByIdTask.AsTask);

			// then
			actaulProfileDependencyException.Should().BeEquivalentTo(
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
		public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfDatabaseUpdateErrorOccursAndLogItAsync()
		{
			// given
			Guid someId = Guid.NewGuid();
			var serviceException = new Exception();

			var failedProfileServiceException =
				new FailedProfileServiceException(serviceException);

			var expectedProfileServiceException =
				new ProfileServiceException(failedProfileServiceException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectProfileByIdAsync(It.IsAny<Guid>()))
					.ThrowsAsync(serviceException);

			// when
			ValueTask<Profile> retrieveProfileByIdTask =
				this.profileService.RetrieveProfileByIdAsync(someId);

			ProfileServiceException actualProfileServiceException =
				await Assert.ThrowsAsync<ProfileServiceException>(
					retrieveProfileByIdTask.AsTask);

			// then
			actualProfileServiceException.Should().BeEquivalentTo(
				expectedProfileServiceException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectProfileByIdAsync(It.IsAny<Guid>()),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
			   broker.LogError(It.Is(SameExceptionAs(
				   expectedProfileServiceException))),
						Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
