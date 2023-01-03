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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidPostId = Guid.Empty;
            Guid invalidProfileId = Guid.Empty;

            var invalidPostImpressionException = new InvalidPostImpressionException();

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.PostId),
                values: "Id is required");

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.ProfileId),
                values: "Id is required");

            var expectedPostImpressionValidationException = new
                PostImpressionValidationException(invalidPostImpressionException);

            // when
            ValueTask<PostImpression> retrievePostImpressionByIdTask =
                this.postImpressionService.RetrievePostImpressionByIdAsync(invalidPostId, invalidProfileId);

            PostImpressionValidationException actualPostImpressionValidationException =
                await Assert.ThrowsAsync<PostImpressionValidationException>(
                    retrievePostImpressionByIdTask.AsTask);

            // then
            actualPostImpressionValidationException.Should().BeEquivalentTo(expectedPostImpressionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(invalidPostId, invalidProfileId),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfTeamIsNotFoundAndLogItAsync()
        {
            //given
            Guid somePostId = Guid.NewGuid();
            Guid someProfileId = Guid.NewGuid();
            PostImpression noPostImpression = null;

            var notFoundPostImpressionException =
                new NotFoundPostImpressionException(somePostId, someProfileId);

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(notFoundPostImpressionException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(somePostId, someProfileId))
                    .ReturnsAsync(noPostImpression);

            //when
            ValueTask<PostImpression> retrievePostImpressionByIdTask =
                this.postImpressionService.RetrievePostImpressionByIdAsync(somePostId, someProfileId);

            PostImpressionValidationException actualPostImpressionValidationException =
                await Assert.ThrowsAsync<PostImpressionValidationException>(
                    retrievePostImpressionByIdTask.AsTask);

            // then
            actualPostImpressionValidationException.Should().BeEquivalentTo(expectedPostImpressionValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(somePostId, someProfileId),
                    Times.Once());

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