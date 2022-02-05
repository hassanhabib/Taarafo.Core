// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Profile randomProfile = CreateRandomProfile(dateTime);
            SqlException sqlException = GetSqlException();

            var failedProfileStorageException =
                new FailedProfileStorageException(sqlException);

            var expectedProfileDependencyException =
                new ProfileDependencyException(failedProfileStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProfileAsync(randomProfile))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Profile> addProfileTask =
                this.profileService.AddProfileAsync(randomProfile);

            // then
            await Assert.ThrowsAsync<ProfileDependencyException>(() =>
                addProfileTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProfileAsync(It.IsAny<Profile>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProfileDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfProfileAlreadyExsitsAndLogItAsync()
        {
            // given
            Profile randomProfile = CreateRandomProfile();
            Profile alreadyExistsProfile = randomProfile;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsProfileException =
                new AlreadyExistsProfileException(duplicateKeyException);

            var expectedProfileDependencyValidationException =
                new ProfileDependencyValidationException(alreadyExistsProfileException);

            this.dateTimeBrokerMock.Setup(broker =>
              broker.GetCurrentDateTimeOffset())
                  .Throws(duplicateKeyException);

            // when
            ValueTask<Profile> addProfileTask =
                this.profileService.AddProfileAsync(alreadyExistsProfile);

            // then
            await Assert.ThrowsAsync<ProfileDependencyValidationException>(() =>
                addProfileTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProfileAsync(It.IsAny<Profile>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProfileDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Profile someProfile = CreateRandomProfile();

            var databaseUpdateException =
                new DbUpdateException();

            var failedProfileStorageException =
                new FailedProfileStorageException(databaseUpdateException);

            var expectedProfileDependencyException =
                new ProfileDependencyException(failedProfileStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Profile> addProfileTask =
                this.profileService.AddProfileAsync(someProfile);

            // then
            await Assert.ThrowsAsync<ProfileDependencyException>(() =>
               addProfileTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProfileAsync(It.IsAny<Profile>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProfileDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
