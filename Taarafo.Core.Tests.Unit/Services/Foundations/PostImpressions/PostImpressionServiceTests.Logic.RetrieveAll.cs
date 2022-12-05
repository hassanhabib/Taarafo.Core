// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.PostImpressions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostImpressions
{
    public partial class PostImpressionServiceTests
    {
        [Fact]
        public void ShouldReturnPostImpressions()
        {
            //given
            IQueryable<PostImpression> randomPostImpressions = CreateRandomPostImpressions();
            IQueryable<PostImpression> storagePostImpressions = randomPostImpressions;
            IQueryable<PostImpression> expectedPostImpressions = storagePostImpressions;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPostImpressions())
                   .Returns(storagePostImpressions);

            //when
            IQueryable<PostImpression> actualPostImpressions =
                this.postImpressionService.RetrieveAllPostImpressions();

            //then
            actualPostImpressions.Should().BeEquivalentTo(expectedPostImpressions);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPostImpressions(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
