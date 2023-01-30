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
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            //given
            SqlException sqlException = CreateSqlException();

            var failedPostPeportStorageException =
                new FailedPostPeportStorageException(sqlException);

            var expectedPostReportDependencyException =
                new PostReportDependencyException(failedPostPeportStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPostReports()).Throws(sqlException);

            //when
            Action retrieveAllPostReportAction = () =>
                this.postReportService.RetrieveAllPostReports();

            PostReportDependencyException actualPostReportDependencyException =
                Assert.Throws<PostReportDependencyException>(retrieveAllPostReportAction);

            //then
            actualPostReportDependencyException.Should().BeEquivalentTo(
                expectedPostReportDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPostReports(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostReportDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
