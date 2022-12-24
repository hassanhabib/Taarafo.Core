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
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenProfileIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomPostId = Guid.NewGuid();
            Guid randomProfileId = default;
            Guid inputPostId = randomPostId;
            Guid inputProfileId = randomProfileId;

            var invalidPostImpressionException = new InvalidPostImpressionException(
                    parameterName: nameof(PostImpression.ProfileId),
                    parameterValue: inputProfileId);

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(invalidPostImpressionException);

            //when
            ValueTask<PostImpression> removePostImpressionTask =
                    this.postImpressionService.RemovePostImpressionByIdAsync(inputPostId, inputProfileId);

            //then
            await Assert.ThrowsAsync<PostImpressionValidationException>(() =>
                removePostImpressionTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdsAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()),
                        Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePostImpressionAsync(
                    It.IsAny<PostImpression>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenPostIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomProfileId = Guid.NewGuid();
            Guid randomPostId = default;
            Guid inputPostId = randomPostId;
            Guid inputProfileId = randomProfileId;

            var invalidPostImpressionException = new InvalidPostImpressionException(
                parameterName: nameof(PostImpression.PostId),
                parameterValue: inputPostId);

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(invalidPostImpressionException);

            //when
            ValueTask<PostImpression> removePostImpressionTask =
                 this.postImpressionService.RemovePostImpressionByIdAsync(inputPostId, inputProfileId);

            //then
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdsAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePostImpressionAsync(It.IsAny<PostImpression>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIdsPostImpressionIsNotFoundAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            PostImpression randomPostImpression = CreateRandomPostImpression(randomDateTime);
            Guid inputPostId = randomPostImpression.PostId;
            Guid inputProfile = randomPostImpression.ProfileId;
            PostImpression nullStoragePostImpression = null;

            var notFoundPostImpressionException =
                new NotFoundPostImpressionException(inputPostId, inputProfile);

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(notFoundPostImpressionException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdsAsync(inputPostId, inputProfile))
                    .ReturnsAsync(nullStoragePostImpression);

            //when
            ValueTask<PostImpression> removePostImpressionTask=
                this.postImpressionService.RemovePostImpressionByIdAsync(inputPostId, inputProfile);

            PostImpressionValidationException actualPostImpressionValidationException =
                await Assert.ThrowsAsync<PostImpressionValidationException>(
                    removePostImpressionTask.AsTask);

            //then
            actualPostImpressionValidationException.Should().BeEquivalentTo(
                expectedPostImpressionValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdsAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePostImpressionAsync(It.IsAny<PostImpression>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }
    }
}
