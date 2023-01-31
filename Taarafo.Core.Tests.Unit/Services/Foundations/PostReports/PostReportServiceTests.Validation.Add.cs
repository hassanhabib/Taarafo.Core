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
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            // given
            PostReport nullPostReport = null;
            var nullPostReportException = new NullPostReportException();

            var expectedPostReportValidationException =
                new PostReportValidationException(nullPostReportException);

            // when
            ValueTask<PostReport> addPostReportTask =
                this.postReportService.AddPostReportAsync(nullPostReport);

            PostReportValidationException actualPostReportValidationException =
                await Assert.ThrowsAsync<PostReportValidationException>(
                    addPostReportTask.AsTask);

            // then
            actualPostReportValidationException.Should()
                .BeEquivalentTo(expectedPostReportValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostReportValidationException))), Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfPostReportIsInvalidAndLogItAsync(string invalidString)
        {
            // given
            PostReport invalidPostReport = new PostReport
            {
                Details = invalidString
            };

            var invalidPostReportException = new InvalidPostReportException();

            invalidPostReportException.AddData(
                key: nameof(PostReport.Id),
                values: "Id is required");

            invalidPostReportException.AddData(
                key: nameof(PostReport.Details),
                values: "Text is required");

            invalidPostReportException.AddData(
                key: nameof(PostReport.PostId),
                values: "Id is required");

            invalidPostReportException.AddData(
                key: nameof(PostReport.Post),
                values: "Object is required");

            invalidPostReportException.AddData(
                key: nameof(PostReport.ReporterId),
                values: "Id is required");

            invalidPostReportException.AddData(
                key: nameof(PostReport.Profile),
                values: "Object is required");

            invalidPostReportException.AddData(
                key: nameof(PostReport.CreatedDate),
                values: "Value is required");

            invalidPostReportException.AddData(
                key: nameof(PostReport.UpdatedDate),
                values: "Value is required");

            var expectedPostReportValidationException =
                new PostReportValidationException(invalidPostReportException);

            // when
            ValueTask<PostReport> addPostReportTask =
                this.postReportService.AddPostReportAsync(invalidPostReport);

            PostReportValidationException actualPostReportValidationException =
                await Assert.ThrowsAsync<PostReportValidationException>(
                    addPostReportTask.AsTask);

            // then
            actualPostReportValidationException.Should()
                .BeEquivalentTo(expectedPostReportValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostReportValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostReportAsync(It.IsAny<PostReport>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset anotherRandomDate = GetRandomDateTimeOffset();
            PostReport randomPostReport = CreateRandomPostReport();
            PostReport invalidPostReport = randomPostReport;
            randomPostReport.UpdatedDate = anotherRandomDate;
            var invalidPostReportException = new InvalidPostReportException();

            invalidPostReportException.AddData(
                key: nameof(PostReport.CreatedDate),
                values: $"Date is not the same as {nameof(PostReport.UpdatedDate)}");

            var expectedPostReportValidationException =
                new PostReportValidationException(invalidPostReportException);

            // when
            ValueTask<PostReport> addPostReportTask =
                this.postReportService.AddPostReportAsync(invalidPostReport);

            PostReportValidationException actualPostReportValidationException =
                await Assert.ThrowsAsync<PostReportValidationException>(
                    addPostReportTask.AsTask);

            // then
            actualPostReportValidationException.Should()
                .BeEquivalentTo(expectedPostReportValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostReportValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostReportAsync(It.IsAny<PostReport>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
