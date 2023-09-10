// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.PostReports.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostReports
{
    public partial class PostReportServiceTests
    {
        [Fact]
        private void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            //given
            SqlException sqlException = CreateSqlException();

            var failedPostPeportStorageException =
                new FailedPostReportStorageException(
                    message: "Failed post report storage error occurred, contact support",
                innerException: sqlException);

            var expectedPostReportDependencyException =
                new PostReportDependencyException(
                    message: "Post report dependency validation occurred, please try again.",
                    innerException: failedPostPeportStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPostReports()).Throws(sqlException);

            //when
            Action retrieveAllPostReportAction = () =>
                this.postReportService.RetrieveAllPostReports();

            PostReportDependencyException actualPostReportDependencyException =
                Assert.Throws<PostReportDependencyException>(
                    retrieveAllPostReportAction);

            //then
            actualPostReportDependencyException.Should().BeEquivalentTo(
                expectedPostReportDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPostReports(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostReportDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            //given
            string expectedMessage = GetRandomMessage();
            var serviceException = new Exception(expectedMessage);

            var failedPostReportServiceException =
                new FailedPostReportServiceException(
                    message: "Failed post report service occurred, please contact support.",
                    innerException: serviceException);

            var expectedPostReportServiceException =
                new PostReportServiceException(
                    message: "Post report service error occurred, please contact support.",
                    innerException: failedPostReportServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPostReports()).Throws(serviceException);

            //when
            Action retieveAllPostReportAction = () =>
                this.postReportService.RetrieveAllPostReports();

            PostReportServiceException actualPostReportServiceException =
                Assert.Throws<PostReportServiceException>(
                    retieveAllPostReportAction);

            //then
            actualPostReportServiceException.Should().BeEquivalentTo(
                expectedPostReportServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPostReports(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostReportServiceException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}