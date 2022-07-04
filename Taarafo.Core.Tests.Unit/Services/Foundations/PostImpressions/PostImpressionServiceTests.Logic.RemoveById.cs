// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task ShouldRemovePostImpressionByIdAsync()
        {
            //given
            Guid randomId = Guid.NewGuid();
            Guid inputPostImpressionId = randomId;
            PostImpression randomPostImpression = CreateRandomPostImpression();
            PostImpression storagePostImpression = randomPostImpression;
            PostImpression expectedInputPostImpression = storagePostImpression;
            PostImpression deletedPostImpression = expectedInputPostImpression;
            PostImpression expectedPostImpression = deletedPostImpression.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(inputPostImpressionId))
                    .ReturnsAsync(storagePostImpression);

            this.storageBrokerMock.Setup(broker =>
                broker.DeletePostImpressionAsync(expectedInputPostImpression))
                    .ReturnsAsync(deletedPostImpression);

            //when
            PostImpression actualPostImpression = await this.postImpressionService
                .RemovePostImpressionByIdAsync(inputPostImpressionId);

            //then
            actualPostImpression.Should().BeEquivalentTo(expectedPostImpression);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(inputPostImpressionId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePostImpressionAsync(expectedInputPostImpression),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
