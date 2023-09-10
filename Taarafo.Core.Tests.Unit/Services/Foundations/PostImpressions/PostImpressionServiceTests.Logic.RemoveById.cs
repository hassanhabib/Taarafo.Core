// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.PostImpressions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostImpressions
{
    public partial class PostImpressionServiceTests
    {
        [Fact]
        private async Task ShouldRemovePostImpressionByIdAsync()
        {
            // given
            Guid randomPostId = Guid.NewGuid();
            Guid randomProfileId = Guid.NewGuid();
            Guid inputPostId = randomPostId;
            Guid inputProfileId = randomProfileId;

            PostImpression randomPostImpression =
                CreateRandomPostImpression();

            randomPostImpression.PostId = inputPostId;
            randomPostImpression.ProfileId = inputProfileId;

            PostImpression storagePostImpression = randomPostImpression;
            PostImpression expectedPostImpression = storagePostImpression;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(inputPostId, inputProfileId))
                    .ReturnsAsync(expectedPostImpression);

            this.storageBrokerMock.Setup(broker =>
                broker.DeletePostImpressionAsync(storagePostImpression))
                    .ReturnsAsync(expectedPostImpression);

            // when
            PostImpression actualPostImpression = await this.postImpressionService
                .RemovePostImpressionAsync(randomPostImpression);

            // then
            actualPostImpression.Should().BeEquivalentTo(expectedPostImpression);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(inputPostId, inputProfileId),
                Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePostImpressionAsync(storagePostImpression),
                Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}