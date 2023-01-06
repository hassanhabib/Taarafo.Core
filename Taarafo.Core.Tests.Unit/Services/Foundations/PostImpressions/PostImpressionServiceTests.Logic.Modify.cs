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
            PostImpression randomPostImpression = CreateRandomPostImpression(randomDateTime);
            PostImpression inputPostImpression = randomPostImpression;
            PostImpression storagePostImpression -inputPostImpression;
            storagePostImpression.UpdatedDate = randomPostImpression.CreatedDate;
            PostImpression updatePostImpression = inputPostImpression;
            PostImpression expectedPostImpression = updatePostImpression.DeepClone();
            Guid postId = inputPostImpression.PostId;
            Guid profileId = inputPostImpression.ProfileId;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdsAsync(postId, profileId)).Returns(storageBrokerMock);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdatePostImpressionAsync(inputPostImpression)).Returns(updatePostImpression);

            //when
            PostImpression actualPostImpression =
                await this.postImpressionService.ModifyPostImpressionAsync(inputPostImpression);

            //then
            actualPostImpression.Should().BeEquivalentTo(expectedPostImpression);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdsAsync(postId, profileId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePostImpressionAsync(inputPostImpression), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
