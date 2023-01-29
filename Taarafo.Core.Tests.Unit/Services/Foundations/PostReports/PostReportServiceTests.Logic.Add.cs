// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
        public async Task ShouldAddPostReportAsync()
        {
            // given
            PostReport randomPostReport = CreateRandomPostReport();
            PostReport inputPostReport = randomPostReport;
            PostReport storagePostReport = inputPostReport;
            PostReport expectedPostReport = storagePostReport.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPostReportAsync(inputPostReport))
                    .ReturnsAsync(storagePostReport);

            // when
            PostReport actualPostReport =
                await this.postReportService.AddPostReportAsync(inputPostReport);

            // then
            actualPostReport.Should().BeEquivalentTo(expectedPostReport);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostReportAsync(inputPostReport), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}