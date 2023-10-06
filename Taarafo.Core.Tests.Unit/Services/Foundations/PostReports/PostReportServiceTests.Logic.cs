// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.PostReports;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostReports
{
    public partial class PostReportServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllPostReports()
        {
            //given
            IQueryable<PostReport> randomPostReports = CreateRandomPostReports();
            IQueryable<PostReport> storagePostReport = randomPostReports;
            IQueryable<PostReport> expectedPostReport = storagePostReport;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPostReports()).Returns(storagePostReport);

            //when
            IQueryable<PostReport> actualPostReport = 
                this.postReportService.RetrieveAllPostReports();

            //then
            actualPostReport.Should().BeEquivalentTo(expectedPostReport);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPostReports(), 
                  Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
