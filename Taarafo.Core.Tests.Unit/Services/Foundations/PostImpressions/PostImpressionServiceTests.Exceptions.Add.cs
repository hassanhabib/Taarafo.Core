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
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostImpressions
{
    public partial class PostImpressionServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();

            PostImpression somePostImpression =
                CreateRandomPostImpression(randomDateTime);

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

            // when
            ValueTask<PostImpression> addPostImpressionTask =
                this.postImpressionService.AddPostImpressions(somePostImpression);

            PostImpressionDependencyException actualPostImpressionDependencyException =
                await Assert.ThrowsAsync<PostImpressionDependencyException>(
                    addPostImpressionTask.AsTask);

            // then
            actualPostImpressionDependencyException.Should().BeEquivalentTo(
                expectedPostImpressionDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostImpressionDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostImpressionAsync(
                    It.IsAny<PostImpression>()),
                     Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnAddIfPostImpressionAlreadyExistsAndLogItAsync()
        {
            // given
            PostImpression randomPostImpression = CreateRandomPostImpression();
            PostImpression alreadyExistsPostImpression = randomPostImpression;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsPostImpressionException =
                new AlreadyExistsPostImpressionException(
                    message: "Post impression with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedPostImpressionDependencyValidationException =
                new PostImpressionDependencyValidationException(
                    message: "Post impression dependency validation occurred, please try again.",
                    innerException: alreadyExistsPostImpressionException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<PostImpression> addPostImpressionTask =
                this.postImpressionService.AddPostImpressions(alreadyExistsPostImpression);

            PostImpressionDependencyValidationException actualPostImpressionDependencyValidationException =
                await Assert.ThrowsAsync<PostImpressionDependencyValidationException>(
                    addPostImpressionTask.AsTask);

            // then
            actualPostImpressionDependencyValidationException.Should().BeEquivalentTo(
                expectedPostImpressionDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostImpressionAsync(
                    It.IsAny<PostImpression>()),
                        Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            PostImpression somePostImpression = CreateRandomPostImpression();

            var databaseUpdateException =
                new DbUpdateException();

            var failedPostImpressionStorageException =
                new FailedPostImpressionStorageException(
                    message: "Failed post impression storage error has occurred, contact support.",
                    innerException: databaseUpdateException);

            var expectedPostImpressionDependencyException =
                new PostImpressionDependencyException(
                    message: "Post impression dependency error has occurred, please contact support.",
                    innerException: failedPostImpressionStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<PostImpression> addPostImpressionTask =
                this.postImpressionService.AddPostImpressions(somePostImpression);

            PostImpressionDependencyException actualPostImpressionDependencyException =
                await Assert.ThrowsAsync<PostImpressionDependencyException>(
                    addPostImpressionTask.AsTask);

            // then
            actualPostImpressionDependencyException.Should().BeEquivalentTo(
                expectedPostImpressionDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostImpressionAsync(
                    It.IsAny<PostImpression>()),
                     Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async void ShouldThrowDependencyValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            PostImpression somePostImpression = CreateRandomPostImpression();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidPostImpressionReferenceException =
                new InvalidPostImpressionReferenceException(
                    message: "Invalid post impression reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var expectedPostImpressionDependencyValidationException =
                new PostImpressionDependencyValidationException(
                    message: "Post impression dependency validation occurred, please try again.",
                    innerException: invalidPostImpressionReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<PostImpression> addPostImpressionTask =
                this.postImpressionService.AddPostImpressions(somePostImpression);

            PostImpressionDependencyValidationException actualPostImpressionDependencyValidationException =
                await Assert.ThrowsAsync<PostImpressionDependencyValidationException>(
                    addPostImpressionTask.AsTask);

            // then
            actualPostImpressionDependencyValidationException.Should().BeEquivalentTo(
                expectedPostImpressionDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostImpressionAsync(
                    It.IsAny<PostImpression>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            //given
            PostImpression somePostImpression = CreateRandomPostImpression();
            var serviceException = new Exception();

            var failedPostImpressionServiceException =
                new FailedPostImpressionServiceException(
                    message: "Failed post impression service occurred, please contact support.",
                    innerException: serviceException);

            var expectedPostImpressionServiceException =
                new PostImpressionServiceException(
                    message: "Post impression service error occurred, please contact support.",
                    innerException: failedPostImpressionServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<PostImpression> addPostImpressionTask =
                this.postImpressionService.AddPostImpressions(somePostImpression);

            PostImpressionServiceException actualPostImpressionServiceException =
                await Assert.ThrowsAsync<PostImpressionServiceException>(
                    addPostImpressionTask.AsTask);

            // then
            actualPostImpressionServiceException.Should().BeEquivalentTo(
                expectedPostImpressionServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostImpressionAsync(
                    It.IsAny<PostImpression>()),
                     Times.Never);

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