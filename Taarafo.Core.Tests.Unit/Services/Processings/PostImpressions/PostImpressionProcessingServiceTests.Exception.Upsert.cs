// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.Processings.PostImpressions.Exceptions;
using Xeptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Processings.PostImpressions
{
    public partial class PostImpressionProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnUpsertIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationExceptions)
        {
            //given
            var somePostImpression = CreateRandomPostImpression();

            var expectedPostImpressionProcessingDependencyValidationException =
                new PostImpressionProcessingDependencyValidationException(
                    dependencyValidationExceptions.InnerException as Xeption);

            this.postImpressionServiceMock.Setup(service =>
                service.RetrieveAllPostImpressions()).Throws(dependencyValidationExceptions);

            //when
            ValueTask<PostImpression> upsertPostImpressionTask =
                this.postImpressionProcessingService.UpsertPostImpressionAsync(somePostImpression);

            PostImpressionProcessingDependencyValidationException
                actualPostImpressionProcessingDependencyValidationException =
                    await Assert.ThrowsAsync<PostImpressionProcessingDependencyValidationException>(
                        upsertPostImpressionTask.AsTask);

            //then
            actualPostImpressionProcessingDependencyValidationException.Should().BeEquivalentTo(
                expectedPostImpressionProcessingDependencyValidationException);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionProcessingDependencyValidationException))), Times.Once);

            this.postImpressionServiceMock.Verify(service =>
                service.AddPostImpressions(It.IsAny<PostImpression>()), Times.Never);

            this.postImpressionServiceMock.Verify(service =>
                service.ModifyPostImpressionAsync(It.IsAny<PostImpression>()), Times.Never);

            this.postImpressionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnUpsertIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            //given
            var somePostImpression = CreateRandomPostImpression();

            var expectedPostImpressionProcessingDependencyException =
                new PostImpressionProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.postImpressionServiceMock.Setup(service =>
                service.RetrieveAllPostImpressions()).Throws(dependencyException);

            //when
            ValueTask<PostImpression> upsertPostImpressionTask =
                this.postImpressionProcessingService.UpsertPostImpressionAsync(somePostImpression);

            PostImpressionProcessingDependencyException actualPostImpressionProcessingDependencyException =
                await Assert.ThrowsAsync<PostImpressionProcessingDependencyException>(upsertPostImpressionTask.AsTask);

            //then
            actualPostImpressionProcessingDependencyException.Should().BeEquivalentTo(
                expectedPostImpressionProcessingDependencyException);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionProcessingDependencyException))), Times.Once);

            this.postImpressionServiceMock.Verify(service =>
                service.AddPostImpressions(It.IsAny<PostImpression>()), Times.Never);

            this.postImpressionServiceMock.Verify(service =>
                service.ModifyPostImpressionAsync(It.IsAny<PostImpression>()), Times.Never);

            this.postImpressionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
