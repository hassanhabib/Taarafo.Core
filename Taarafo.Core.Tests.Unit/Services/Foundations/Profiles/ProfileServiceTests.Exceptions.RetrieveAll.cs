// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.Profiles.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Profiles
{
    public partial class ProfileServiceTests
    {
        [Fact]
        private void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedProfileStorageException =
                new FailedProfileStorageException(
                    message: "Failed profile storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedProfileDependencyException =
                new ProfileDependencyException(
                    message: "Profile dependency error occurred, contact support.",
                    innerException: failedProfileStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProfiles())
                    .Throws(sqlException);

            // when
            Action retrieveAllProfileAction = () =>
                this.profileService.RetrieveAllProfiles();

            ProfileDependencyException actualProfileDependencyException =
                Assert.Throws<ProfileDependencyException>(
                    retrieveAllProfileAction);

            // then
            actualProfileDependencyException.Should()
                .BeEquivalentTo(expectedProfileDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProfiles());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProfileDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var faileProfileServiceException =
                new FailedProfileServiceException(
                    message: "Failed profile service occurred, please contact support",
                    innerException: serviceException);

            var expectedProfileServiceException =
                new ProfileServiceException(
                    message: "Profile service error occurred, contact support.",
                    innerException: faileProfileServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProfiles())
                    .Throws(serviceException);

            // when
            Action retrieveAllProfilesAction = () =>
                this.profileService.RetrieveAllProfiles();

            ProfileServiceException actualProfileServiceException =
                Assert.Throws<ProfileServiceException>(
                    retrieveAllProfilesAction);

            // then
            actualProfileServiceException.Should().BeEquivalentTo(
                expectedProfileServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProfiles(),
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