// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnUpsertIfPostImpressionIsNullAndLogItAsync()
        {
            //given
            PostImpression nullPostImpression = null;

            var nullPostImpressionProcessingException =
                new NullPostImpressionProcessingException();

            var expectedPostImpressionProcessingValidationException =
                new PostImpressionProcessingValidationException(
                    nullPostImpressionProcessingException);

            //when
            ValueTask<PostImpression> upsertPostImpressionTask =
                this.postImpressionProcessingService.UpsertPostImpressionAsync(nullPostImpression);

            PostImpressionProcessingValidationException actualPostImpressionProcessingValidationException =
                await Assert.ThrowsAsync<PostImpressionProcessingValidationException>(
                    upsertPostImpressionTask.AsTask);

            //then
            actualPostImpressionProcessingValidationException.Should().BeEquivalentTo(
                expectedPostImpressionProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionProcessingValidationException))), Times.Once);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(), Times.Never);

            this.postImpressionServiceMock.Verify(service =>
                service.AddPostImpressions(It.IsAny<PostImpression>()), Times.Never);

            this.postImpressionServiceMock.Verify(service =>
                service.ModifyPostImpressionAsync(It.IsAny<PostImpression>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.postImpressionServiceMock.VerifyNoOtherCalls();
        }
    }
}
