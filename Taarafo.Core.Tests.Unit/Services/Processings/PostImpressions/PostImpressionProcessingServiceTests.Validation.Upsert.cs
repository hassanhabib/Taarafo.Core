// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.Processings.PostImpressions.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Processings.PostImpressions
{
    public partial class PostImpressionProcessingServiceTests
    {
        [Fact]
        private async Task ShouldThrowValidationExceptionOnUpsertIfPostImpressionIsNullAndLogItAsync()
        {
            //given
            PostImpression nullPostImpression = null;

            var nullPostImpressionProcessingException =
                new NullPostImpressionProcessingException();

            var expectedPostImpressionProcessingValidationException =
                new PostImpressionProcessingValidationException(
                    message: "Post Impression validation error occurred, please try again.",
                    innerException: nullPostImpressionProcessingException);

            //when
            ValueTask<PostImpression> upsertPostImpressionTask =
                this.postImpressionProcessingService.UpsertPostImpressionAsync(
                    nullPostImpression);

            PostImpressionProcessingValidationException
                actualPostImpressionProcessingValidationException =
                await Assert.ThrowsAsync<PostImpressionProcessingValidationException>(
                    upsertPostImpressionTask.AsTask);

            //then
            actualPostImpressionProcessingValidationException.Should().BeEquivalentTo(
                expectedPostImpressionProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionProcessingValidationException))),
                    Times.Once);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(),
                Times.Never);

            this.postImpressionServiceMock.Verify(service =>
                service.AddPostImpressions(It.IsAny<PostImpression>()),
                Times.Never);

            this.postImpressionServiceMock.Verify(service =>
                service.ModifyPostImpressionAsync(It.IsAny<PostImpression>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.postImpressionServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnUpsertIfPostImpressionIdsIsInvalidAndLogItAsync()
        {
            // given
            var invalidPostImpression = CreateRandomPostImpression();
            invalidPostImpression.PostId = Guid.Empty;
            invalidPostImpression.ProfileId = Guid.Empty;

            var invalidPostImpressionProcessingException =
                new InvalidPostImpressionProcessingException();

            invalidPostImpressionProcessingException.AddData(
                key: nameof(PostImpression.PostId),
                values: "Id is required");

            invalidPostImpressionProcessingException.AddData(
                key: nameof(PostImpression.ProfileId),
                values: "Id is required");

            var expectedPostImpressionProcessingValidationException =
                new PostImpressionProcessingValidationException(
                    message: "Post Impression validation error occurred, please try again.",
                    innerException: invalidPostImpressionProcessingException);

            // when
            ValueTask<PostImpression> upsertPostImpressionTask =
                this.postImpressionProcessingService.UpsertPostImpressionAsync(
                    invalidPostImpression);

            PostImpressionProcessingValidationException
                actualPostImpressionProcessingValidationException =
                await Assert.ThrowsAsync<PostImpressionProcessingValidationException>(
                    upsertPostImpressionTask.AsTask);

            // then
            actualPostImpressionProcessingValidationException.Should().BeEquivalentTo(
                expectedPostImpressionProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionProcessingValidationException))),
                    Times.Once);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(), Times.Never);

            this.postImpressionServiceMock.Verify(service =>
                service.AddPostImpressions(It.IsAny<PostImpression>()),
                Times.Never);

            this.postImpressionServiceMock.Verify(service =>
                service.ModifyPostImpressionAsync(It.IsAny<PostImpression>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.postImpressionServiceMock.VerifyNoOtherCalls();
        }
    }
}