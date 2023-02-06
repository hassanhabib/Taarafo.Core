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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            PostReport somePostReport = CreateRandomPostReport();
            SqlException sqlException = CreateSqlException();

            var failedPostReportStorageException =
                new FailedPostReportStorageException(sqlException);

            var expectedPostReportDependencyException =
                new PostReportDependencyException(failedPostReportStorageException);

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
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostReportDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            PostReport somePostReport = CreateRandomPostReport();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsPostReportException =
                new AlreadyExistsPostReportException(duplicateKeyException);

            var expectedPostReportDependencyValidationException =
                new PostReportDependencyValidationException(alreadyExistsPostReportException);

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
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostReportDependencyValidationException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDbConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            PostReport somePostReport = CreateRandomPostReport();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedPostReportException =
                new LockedPostReportException(dbUpdateConcurrencyException);

            var expectedPostReportDependencyValidation =
                new PostReportDependencyValidationException(lockedPostReportException);

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
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostReportDependencyValidation))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            PostReport somePostReport = CreateRandomPostReport();
            var serviceException = new Exception();

            var failedPostReportServiceException =
                new FailedPostReportServiceException(serviceException);

            var expectedPostReportServiceException =
                new PostReportServiceException(failedPostReportServiceException);

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