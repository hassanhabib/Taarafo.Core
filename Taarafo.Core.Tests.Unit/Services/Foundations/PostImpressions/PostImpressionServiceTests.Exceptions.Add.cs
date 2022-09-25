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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            PostImpression somePostImpression = CreateRandomPostImpression(randomDateTime);
            SqlException sqlException = GetSqlException();

            var failedPostImpressionStorageException =
                new FailedPostImpressionStorageException(sqlException);

            var expectedPostImpressionDependencyException =
                new PostImpressionDependencyException(failedPostImpressionStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            //when
            ValueTask<PostImpression> addPostImpressionTask =
                this.postImpressionService.AddPostImpressions(somePostImpression);

            PostImpressionDependencyException actualPostImpressionDependencyException =
                await Assert.ThrowsAsync<PostImpressionDependencyException>(
                    addPostImpressionTask.AsTask);

            //then
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
                broker.InsertPostImpressionAsync(It.IsAny<PostImpression>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfPostImpressionAlreadyExistsAndLogItAsync()
        {
            //given
            PostImpression randomPostImpression = CreateRandomPostImpression();
            PostImpression alreadyExistsPostImpression = randomPostImpression;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsPostImpressionException =
                new AlreadyExistsPostImpressionException(duplicateKeyException);

            var expectedPostImpressionDependencyValidationException =
                new PostImpressionDependencyValidationException(alreadyExistsPostImpressionException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            //when
            ValueTask<PostImpression> addPostImpressionTask =
                this.postImpressionService.AddPostImpressions(alreadyExistsPostImpression);

            PostImpressionDependencyValidationException actualPostImpressionDependencyValidationException =
                await Assert.ThrowsAsync<PostImpressionDependencyValidationException>(
                    addPostImpressionTask.AsTask);

            //then
            actualPostImpressionDependencyValidationException.Should().BeEquivalentTo(
                expectedPostImpressionDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostImpressionAsync(It.IsAny<PostImpression>()),
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
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            //given
            PostImpression somePostImpression = CreateRandomPostImpression();

            var databaseUpdateException =
                new DbUpdateException();

            var failedPostImpressionStorageException =
                new FailedPostImpressionStorageException(databaseUpdateException);

            var expectedPostImpressionDependencyException =
                new PostImpressionDependencyException(failedPostImpressionStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            //when
            ValueTask<PostImpression> addPostImpressionTask =
                this.postImpressionService.AddPostImpressions(somePostImpression);

            PostImpressionDependencyException actualPostImpressionDependencyException =
                await Assert.ThrowsAsync<PostImpressionDependencyException>(
                    addPostImpressionTask.AsTask);

            //then
            actualPostImpressionDependencyException.Should().BeEquivalentTo(
                expectedPostImpressionDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostImpressionAsync(It.IsAny<PostImpression>()),
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
        public async void ShouldThrowDependencyValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            //given
            PostImpression somePostImpression = CreateRandomPostImpression();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidPostImpressionReferenceException =
                new InvalidPostImpressionReferenceException(foreignKeyConstraintConflictException);

            var expectedPostImpressionDependencyValidationException =
                new PostImpressionDependencyValidationException(invalidPostImpressionReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            //when
            ValueTask<PostImpression> addPostImpressionTask =
                this.postImpressionService.AddPostImpressions(somePostImpression);

            PostImpressionDependencyValidationException actualPostImpressionDependencyValidationException =
                await Assert.ThrowsAsync<PostImpressionDependencyValidationException>(
                    addPostImpressionTask.AsTask);

            //then
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
                broker.InsertPostImpressionAsync(It.IsAny<PostImpression>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            //given
            PostImpression somePostImpression = CreateRandomPostImpression();
            var serviceException = new Exception();

            var failedPostImpressionServiceException =
                new FailedPostImpressionServiceException(serviceException);

            var expectedPostImpressionServiceException =
                new PostImpressionServiceException(failedPostImpressionServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            //when
            ValueTask<PostImpression> addPostImpressionTask =
                this.postImpressionService.AddPostImpressions(somePostImpression);

            PostImpressionServiceException actualPostImpressionServiceException =
                await Assert.ThrowsAsync<PostImpressionServiceException>(
                    addPostImpressionTask.AsTask);

            //then
            actualPostImpressionServiceException.Should().BeEquivalentTo(
                expectedPostImpressionServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostImpressionAsync(It.IsAny<PostImpression>()),
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
