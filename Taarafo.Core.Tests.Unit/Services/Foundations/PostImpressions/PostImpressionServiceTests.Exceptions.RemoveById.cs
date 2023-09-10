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
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostImpressions
{
    public partial class PostImpressionServiceTests
    {
        [Fact]
        private async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
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

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()))
                .ThrowsAsync(serviceException);

            // when
            ValueTask<PostImpression> deletePostImpressionTask =
                 this.postImpressionService.RemovePostImpressionAsync(somePostImpression);

            PostImpressionServiceException actualPostImpressionServiceException =
                await Assert.ThrowsAsync<PostImpressionServiceException>(
                    deletePostImpressionTask.AsTask);

            // then
            actualPostImpressionServiceException.Should().BeEquivalentTo(
                expectedPostImpressionServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionServiceException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            PostImpression somePostImpression = CreateRandomPostImpression();
            SqlException sqlException = GetSqlException();

            var failedPostImpressionStorageException =
                new FailedPostImpressionStorageException(
                    message: "Failed post impression storage error has occurred, contact support.",
                    innerException: sqlException);

            var expectedPostImpressionDependencyException =
                new PostImpressionDependencyException(
                    message: "Post impression dependency error has occurred, please contact support.",
                    innerException: failedPostImpressionStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>())).ThrowsAsync(sqlException);

            // when
            ValueTask<PostImpression> deletePostImpressionTask =
                this.postImpressionService.RemovePostImpressionAsync(somePostImpression);

            PostImpressionDependencyException actualPostImpressionDependencyException =
                await Assert.ThrowsAsync<PostImpressionDependencyException>(
                    deletePostImpressionTask.AsTask);

            // then
            actualPostImpressionDependencyException.Should().BeEquivalentTo(
                expectedPostImpressionDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostImpressionDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            PostImpression somePostImpression = CreateRandomPostImpression();

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
                broker.SelectPostImpressionByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()))
                        .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<PostImpression> removePostImpressionByIdTask =
                this.postImpressionService.RemovePostImpressionAsync(somePostImpression);

            PostImpressionDependencyValidationException actualPostImpressionDependencyValidationException =
                await Assert.ThrowsAsync<PostImpressionDependencyValidationException>(
                    removePostImpressionByIdTask.AsTask);

            // then
            actualPostImpressionDependencyValidationException.Should().BeEquivalentTo(
                expectedPostImpressionDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectPostImpressionByIdAsync(
                   It.IsAny<Guid>(),
                   It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePostImpressionAsync(
                    It.IsAny<PostImpression>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}