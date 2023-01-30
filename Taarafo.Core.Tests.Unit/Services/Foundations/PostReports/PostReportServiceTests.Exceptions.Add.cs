// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
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
    }
}