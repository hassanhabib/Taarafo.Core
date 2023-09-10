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
using Taarafo.Core.Models.PostReports;
using Taarafo.Core.Models.PostReports.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostReports
{
    public partial class PostReportServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            PostReport somePostReport = CreateRandomPostReport();
            SqlException sqlException = CreateSqlException();

            var failedPostReportStorageException =
                new FailedPostReportStorageException(
                    message: "Failed post report storage error occurred, contact support",
                    innerException: sqlException);

            var expectedPostReportDependencyException =
                new PostReportDependencyException(
                    message: "Post report dependency validation occurred, please try again.",
                    innerException: failedPostReportStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(sqlException);

            // when
            ValueTask<PostReport> addPostReportTask =
                this.postReportService.AddPostReportAsync(somePostReport);

            PostReportDependencyException actualPostReportDependencyException =
                await Assert.ThrowsAsync<PostReportDependencyException>(
                    addPostReportTask.AsTask);

            // then
            actualPostReportDependencyException.Should()
                .BeEquivalentTo(expectedPostReportDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostReportDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            PostReport somePostReport = CreateRandomPostReport();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsPostReportException =
                new AlreadyExistsPostReportException(
                    message: "PostReport already exists.",
                    innerException: duplicateKeyException);

            var expectedPostReportDependencyValidationException =
                new PostReportDependencyValidationException(
                    message: "PostReport dependency validation error occurred, fix the errors and try again.",
                    innerException: alreadyExistsPostReportException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(duplicateKeyException);

            // when
            ValueTask<PostReport> addPostReportTask =
                this.postReportService.AddPostReportAsync(somePostReport);

            PostReportDependencyValidationException actualPostReportDependencyValidationException =
                await Assert.ThrowsAsync<PostReportDependencyValidationException>(
                    addPostReportTask.AsTask);

            // then
            actualPostReportDependencyValidationException.Should()
                .BeEquivalentTo(expectedPostReportDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostReportDependencyValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnAddIfDbConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            PostReport somePostReport = CreateRandomPostReport();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedPostReportException =
                new LockedPostReportException(
                    message: "PostReport is locked, please try again.",
                    innerException: dbUpdateConcurrencyException);

            var expectedPostReportDependencyValidation =
                new PostReportDependencyValidationException(
                    message: "PostReport dependency validation error occurred, fix the errors and try again.",
                    innerException: lockedPostReportException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(dbUpdateConcurrencyException);

            // when
            ValueTask<PostReport> addPostReportTask =
                this.postReportService.AddPostReportAsync(somePostReport);

            PostReportDependencyValidationException actualPostReportDependencyValidation =
                await Assert.ThrowsAsync<PostReportDependencyValidationException>(
                    addPostReportTask.AsTask);

            // then
            actualPostReportDependencyValidation.Should()
                .BeEquivalentTo(expectedPostReportDependencyValidation);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostReportDependencyValidation))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            PostReport somePostReport = CreateRandomPostReport();
            var serviceException = new Exception();

            var failedPostReportServiceException =
                new FailedPostReportServiceException(
                    message: "Failed post report service occurred, please contact support.",
                    innerException: serviceException);

            var expectedPostReportServiceException =
                new PostReportServiceException(
                    message: "Post report service error occurred, please contact support.",
                    innerException: failedPostReportServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(serviceException);

            // when
            ValueTask<PostReport> addPostReportTask =
                this.postReportService.AddPostReportAsync(somePostReport);

            PostReportServiceException actualPostReportServiceException =
                await Assert.ThrowsAsync<PostReportServiceException>(
                    addPostReportTask.AsTask);

            // then
            actualPostReportServiceException.Should()
                .BeEquivalentTo(expectedPostReportServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostReportServiceException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}