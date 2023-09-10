﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostImpressions
{
    public partial class PostImpressionServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();

            PostImpression randomPostImpression =
                CreateRandomModifyPostImpression(randomDateTime);

            PostImpression somePostImpression = randomPostImpression;
            Guid postId = somePostImpression.PostId;
            Guid profileId = somePostImpression.ProfileId;
            SqlException sqlException = GetSqlException();

            var failedPostImpressionStorageException =
                new FailedPostImpressionStorageException(
                    message: "Failed post impression storage error has occurred, contact support.",
                    innerException: sqlException);

            var expectedPostImpressionDependencyException =
                new PostImpressionDependencyException(
                    message: "Post impression dependency error has occurred, please contact support.",
                    innerException: failedPostImpressionStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Throws(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(postId, profileId))
                .Throws(sqlException);

            // when
            ValueTask<PostImpression> modifyPostImpression =
                this.postImpressionService.ModifyPostImpressionAsync(somePostImpression);

            PostImpressionDependencyException actualPostImpressionDependencyException =
                await Assert.ThrowsAsync<PostImpressionDependencyException>(
                    modifyPostImpression.AsTask);

            // then
            actualPostImpressionDependencyException.Should().BeEquivalentTo(
                expectedPostImpressionDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostImpressionDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(postId, profileId),
                Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePostImpressionAsync(somePostImpression),
                Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();

            PostImpression randomPostImpression =
                CreateRandomPostImpression(randomDateTime);

            PostImpression somePostIpression = randomPostImpression;
            Guid postId = somePostIpression.PostId;
            Guid profileId = somePostIpression.ProfileId;

            somePostIpression.CreatedDate =
                randomDateTime.AddMinutes(minutesInPast);

            var databaseUpdateException = new DbUpdateException();

            var failedPostImpressionStorageException =
                new FailedPostImpressionStorageException(
                    message: "Failed post impression storage error has occurred, contact support.",
                    innerException: databaseUpdateException);

            var expectedPostImpressionDependencyException =
                new PostImpressionDependencyException(
                    message: "Post impression dependency error has occurred, please contact support.",
                    innerException: failedPostImpressionStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(postId, profileId))
                .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<PostImpression> modifyPostImpression =
                this.postImpressionService.ModifyPostImpressionAsync(somePostIpression);

            PostImpressionDependencyException actualPostImpressionDependencyException =
                await Assert.ThrowsAsync<PostImpressionDependencyException>(
                    modifyPostImpression.AsTask);

            // then
            actualPostImpressionDependencyException.Should().BeEquivalentTo(
                expectedPostImpressionDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(postId, profileId),
                Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();

            PostImpression randomPostImpression =
                CreateRandomPostImpression(randomDateTime);

            PostImpression somePostImpression = randomPostImpression;

            randomPostImpression.CreatedDate =
                randomDateTime.AddMinutes(minutesInPast);

            Guid postId = somePostImpression.PostId;
            Guid profileId = somePostImpression.ProfileId;

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedPostImpressionException =
                new LockedPostImpressionException(
                    message: "Locked post impression record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedPostImpressionDependencyValidationException =
                new PostImpressionDependencyValidationException(
                    message: "Post impression dependency validation occurred, please try again.",
                    innerException: lockedPostImpressionException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(postId, profileId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            //  when
            ValueTask<PostImpression> modifyPostImpression =
                this.postImpressionService.ModifyPostImpressionAsync(somePostImpression);

            PostImpressionDependencyValidationException actualPostImpressionDependencyValidationException =
                await Assert.ThrowsAsync<PostImpressionDependencyValidationException>(modifyPostImpression.AsTask);

            // then
            actualPostImpressionDependencyValidationException.Should().BeEquivalentTo(
                expectedPostImpressionDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(postId, profileId),
                Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionDependencyValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();

            PostImpression randomPostImpression =
                CreateRandomPostImpression(randomDateTime);

            PostImpression somePostImpression = randomPostImpression;
            Guid postId = somePostImpression.PostId;
            Guid profileId = somePostImpression.ProfileId;

            somePostImpression.CreatedDate =
                randomDateTime.AddMinutes(minutesInPast);

            var serviceException = new Exception();

            var failedPostImpressionService =
                new FailedPostImpressionServiceException(
                    message: "Failed post impression service occurred, please contact support.",
                    innerException: serviceException);

            var expectedPostImpressionServiceException =
                new PostImpressionServiceException(
                    message: "Post impression service error occurred, please contact support.",
                    innerException: failedPostImpressionService);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(postId, profileId))
                .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<PostImpression> modifyPostImpression =
                this.postImpressionService.ModifyPostImpressionAsync(somePostImpression);

            PostImpressionServiceException actualPostImpressionServiceException =
                await Assert.ThrowsAsync<PostImpressionServiceException>(
                    modifyPostImpression.AsTask);

            // then
            actualPostImpressionServiceException.Should().BeEquivalentTo(
                expectedPostImpressionServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(postId, profileId),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionServiceException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}