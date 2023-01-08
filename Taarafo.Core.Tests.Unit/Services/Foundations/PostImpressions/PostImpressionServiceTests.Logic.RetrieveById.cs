// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.PostImpressions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostImpressions
{
    public partial class PostImpressionServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveGroupPostByIdAsync()
        {
            // given
            Guid randomPostId = Guid.NewGuid();
            Guid randomProfileId = Guid.NewGuid();
            Guid inputPostId = randomPostId;
            Guid inputProfileId = randomProfileId;
            PostImpression randomPostImpression = CreateRandomPostImpression();
            PostImpression storagePostImpression = randomPostImpression;
            PostImpression expectedPostImpression = storagePostImpression.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(inputPostId, inputProfileId))
                    .ReturnsAsync(storagePostImpression);

            // when
            PostImpression actualPostImpression =
                await this.postImpressionService.RetrievePostImpressionByIdAsync(inputPostId, inputProfileId);

            // then
            actualPostImpression.Should().BeEquivalentTo(expectedPostImpression);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(inputPostId, inputProfileId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}