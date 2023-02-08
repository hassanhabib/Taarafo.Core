// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid somePostReportId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedPostReportStorageException =
                new FailedPostReportStorageException(sqlException);

            var expectedPostReportDependencyException =
                new PostReportDependencyException(failedPostReportStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostReportByIdAsync(somePostReportId))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<PostReport> retrievePostReportByIdTask =
                this.postReportService.RetrievePostReportByIdAsync(somePostReportId);

            PostReportDependencyException actualPostReportDependencyException =
                await Assert.ThrowsAsync<PostReportDependencyException>(
                    retrievePostReportByIdTask.AsTask);

            //then
            actualPostReportDependencyException.Should().BeEquivalentTo(
                   expectedPostReportDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostReportByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostReportDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
