// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPostReportAsync(It.IsAny<PostReport>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<PostReport> addPostReportTask =
                this.postReportService.AddPostReportAsync(somePostReport);

            PostReportDependencyException actualPostReportDependencyException =
                await Assert.ThrowsAsync<PostReportDependencyException>(
                    addPostReportTask.AsTask);

            // then
            actualPostReportDependencyException.Should()
                .BeEquivalentTo(expectedPostReportDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostReportAsync(It.IsAny<PostReport>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostReportDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
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

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPostReportAsync(It.IsAny<PostReport>()))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<PostReport> addPostReportTask =
                this.postReportService.AddPostReportAsync(somePostReport);

            PostReportDependencyValidationException actualPostReportDependencyValidationException =
                await Assert.ThrowsAsync<PostReportDependencyValidationException>(
                    addPostReportTask.AsTask);

            // then
            actualPostReportDependencyValidationException.Should()
                .BeEquivalentTo(expectedPostReportDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
              broker.InsertPostReportAsync(It.IsAny<PostReport>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostReportDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
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

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPostReportAsync(It.IsAny<PostReport>()))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<PostReport> addPostReportTask =
                this.postReportService.AddPostReportAsync(somePostReport);

            PostReportDependencyValidationException actualPostReportDependencyValidation =
                await Assert.ThrowsAsync<PostReportDependencyValidationException>(
                    addPostReportTask.AsTask);

            // then
            actualPostReportDependencyValidation.Should()
                .BeEquivalentTo(expectedPostReportDependencyValidation);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostReportAsync(It.IsAny<PostReport>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostReportDependencyValidation))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}