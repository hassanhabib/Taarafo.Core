// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.PostImpressions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Processings.PostImpressions
{
    public partial class PostImpressionProcessingServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllPostImpressions()
        {
            //given
            IQueryable<PostImpression> randomPostImpressions = CreateRandomPostImpressions();
            IQueryable<PostImpression> storagePostImpressions = randomPostImpressions;
            IQueryable<PostImpression> expectedPostImpressions = storagePostImpressions;

            this.postImpressionServiceMock.Setup(service =>
                service.RetrieveAllPostImpressions())
                    .Returns(expectedPostImpressions);

            //when
            IQueryable<PostImpression> actualPostImpression =
                this.postImpressionProcessingService.RetrieveAllPostImpressionsAsync();

            //then
            actualPostImpression.Should().BeEquivalentTo(expectedPostImpressions);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(), Times.Once);

            this.postImpressionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
