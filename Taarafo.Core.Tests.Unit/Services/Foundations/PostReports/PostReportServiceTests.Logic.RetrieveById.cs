// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.PostReports;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostReports
{
    public partial class PostReportServiceTests
    {
        [Fact]
        public async Task ShouldRetrievePostReportByIdAsync()
        {
            // given
            Guid randomPostReportId = Guid.NewGuid();
            Guid inputPostReportId = randomPostReportId;
            PostReport randomPostReport = CreateRandomPostReport();
            PostReport storagePostReport = randomPostReport;
            PostReport expectedPostReport = storagePostReport.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostReportByIdAsync(inputPostReportId))
                    .ReturnsAsync(storagePostReport);

            // when
            PostReport actualPostReport =
                await this.postReportService.RetrievePostReportByIdAsync(inputPostReportId);

            // then
            actualPostReport.Should().BeEquivalentTo(expectedPostReport);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostReportByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
