// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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

            // then
            await Assert.ThrowsAsync<ProfileDependencyValidationException>(() =>
                removeProfileByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProfileByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProfileDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProfileAsync(It.IsAny<Profile>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
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
            ValueTask<Profile> deleteProfileTask =
                this.profileService.RemoveProfileByIdAsync(someProfileId);

            // then
            await Assert.ThrowsAsync<ProfileDependencyException>(() =>
                deleteProfileTask.AsTask());

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

    }
}
