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
        public async Task ShouldModifyPostImpressionAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            PostImpression randomPostImpression = CreateRandomModifyPostImpression(randomDateTime);
            PostImpression inputPostImpression = randomPostImpression;
            PostImpression storagePostImpression = inputPostImpression.DeepClone();
            storagePostImpression.UpdatedDate = randomPostImpression.CreatedDate;
            PostImpression updatePostImpression = inputPostImpression;
            PostImpression expectedPostImpression = updatePostImpression.DeepClone();
            Guid postId = inputPostImpression.PostId;
            Guid profileId = inputPostImpression.ProfileId;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(postId, profileId))
                    .ReturnsAsync(storagePostImpression);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdatePostImpressionAsync(inputPostImpression))
                    .ReturnsAsync(updatePostImpression);

            //when
            PostImpression actualPostImpression =
                await this.postImpressionService.ModifyPostImpressionAsync(inputPostImpression);

            //then
            actualPostImpression.Should().BeEquivalentTo(expectedPostImpression);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(postId, profileId), 
                  Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePostImpressionAsync(inputPostImpression), 
                  Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
