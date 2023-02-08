// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.PostReports;
using Taarafo.Core.Models.PostReports.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostReports
{
    public partial class PostReportServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidPostReportId = Guid.Empty;

            var invalidPostReportException = new InvalidPostReportException();

            invalidPostReportException.AddData(
                key: nameof(PostReport.Id),
                values: "Id is required");

            var expectedPostReportValidationException = new
                PostReportValidationException(invalidPostReportException);

            // when
            ValueTask<PostReport> retrievePostReportByIdTask =
                this.postReportService.RetrievePostReportByIdAsync(invalidPostReportId);

            PostReportValidationException actualPostReportValidationException =
                await Assert.ThrowsAsync<PostReportValidationException>(
                    retrievePostReportByIdTask.AsTask);

            // then
            actualPostReportValidationException.Should()
                .BeEquivalentTo(expectedPostReportValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostReportValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostReportByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfPostReportIsNotFoundAndLogItAsync()
        {
            //given
            Guid somePostReportId = Guid.NewGuid();
            PostReport nullPostReport = null;

            var notFoundPostReportException =
                new NotFoundPostReportException(somePostReportId);

            var expectedPostReportValidationException =
                new PostReportValidationException(notFoundPostReportException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostReportByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(nullPostReport);

            //when
            ValueTask<PostReport> retrievePostReportByIdTask =
                this.postReportService.RetrievePostReportByIdAsync(somePostReportId);

            PostReportValidationException actualPostReportValidationException =
                await Assert.ThrowsAsync<PostReportValidationException>(
                    retrievePostReportByIdTask.AsTask);

            // then
            actualPostReportValidationException.Should()
                .BeEquivalentTo(expectedPostReportValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostReportByIdAsync(It.IsAny<Guid>()), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostReportValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
