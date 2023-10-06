// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostImpressions
{
    public partial class PostImpressionServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            //given
            var postImpression = new PostImpression();
            var invalidPostImpressionException = new InvalidPostImpressionException();

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.PostId),
                values: "Id is required");

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.ProfileId),
                values: "Id is required");

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(invalidPostImpressionException);

            //when
            ValueTask<PostImpression> removePostImpressionTask =
                this.postImpressionService.RemovePostImpressionAsync(postImpression);

            PostImpressionValidationException actualPostImpressionValidationException =
                await Assert.ThrowsAsync<PostImpressionValidationException>(
                    removePostImpressionTask.AsTask);

            //then
            actualPostImpressionValidationException.Should().BeEquivalentTo(
                expectedPostImpressionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))), 
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIdsPostImpressionIsNotFoundAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            PostImpression randomPostImpression = CreateRandomPostImpression(randomDateTime);
            Guid inputPostId = randomPostImpression.PostId;
            Guid inputProfile = randomPostImpression.ProfileId;
            PostImpression nullStoragePostImpression = null;

            var notFoundPostImpressionException =
                new NotFoundPostImpressionException(inputPostId, inputProfile);

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(notFoundPostImpressionException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(inputPostId, inputProfile))
                    .ReturnsAsync(nullStoragePostImpression);

            //when
            ValueTask<PostImpression> removePostImpressionTask =
                this.postImpressionService.RemovePostImpressionAsync(randomPostImpression);

            PostImpressionValidationException actualPostImpressionValidationException =
                await Assert.ThrowsAsync<PostImpressionValidationException>(
                    removePostImpressionTask.AsTask);

            //then
            actualPostImpressionValidationException.Should().BeEquivalentTo(
                expectedPostImpressionValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))), 
                      Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
