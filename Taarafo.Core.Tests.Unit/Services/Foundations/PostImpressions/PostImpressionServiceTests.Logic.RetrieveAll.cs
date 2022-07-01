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
            IQueryable<PostImpression> randomPostImpression = CreateRandomPostImpressions();
            IQueryable<PostImpression> storagePostImpression = randomPostImpression;
            IQueryable<PostImpression> expectedPostImpression = storagePostImpression;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPostImpressions())
                    .Returns(storagePostImpression);

            //when
            IQueryable<PostImpression> actualPostImpressions =
                this.postImpressionService.RetrieveAllPostImpressions();

            //then
            actualPostImpressions.Should().BeEquivalentTo(expectedPostImpression);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPostImpressions(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
