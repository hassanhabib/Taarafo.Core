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
using Xeptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Processings.PostImpressions
{
    public partial class PostImpressionProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        private async Task ShouldThrowDependencyValidationOnUpsertIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationExceptions)
        {
            // given
            var somePostImpression = CreateRandomPostImpression();

            var expectedPostImpressionProcessingDependencyValidationException =
                new PostImpressionProcessingDependencyValidationException(
                    message: "Post Impression dependency validation error occurred, please try again.",
                    innerException: dependencyValidationExceptions.InnerException as Xeption);

            this.postImpressionServiceMock.Setup(service =>
                service.RetrieveAllPostImpressions()).Throws(dependencyValidationExceptions);

            // when
            ValueTask<PostImpression> upsertPostImpressionTask =
                this.postImpressionProcessingService.UpsertPostImpressionAsync(somePostImpression);

            PostImpressionProcessingDependencyValidationException
                actualPostImpressionProcessingDependencyValidationException =
                    await Assert.ThrowsAsync<PostImpressionProcessingDependencyValidationException>(
                        upsertPostImpressionTask.AsTask);

            // then
            actualPostImpressionProcessingDependencyValidationException.Should().BeEquivalentTo(
                expectedPostImpressionProcessingDependencyValidationException);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionProcessingDependencyValidationException))),
                    Times.Once);

            this.postImpressionServiceMock.Verify(service =>
                service.AddPostImpressions(
                    It.IsAny<PostImpression>()),
                    Times.Never);

            this.postImpressionServiceMock.Verify(service =>
                service.ModifyPostImpressionAsync(
                    It.IsAny<PostImpression>()),
                    Times.Never);

            this.postImpressionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        private async Task ShouldThrowDependencyExceptionOnUpsertIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var somePostImpression = CreateRandomPostImpression();

            var expectedPostImpressionProcessingDependencyException =
                new PostImpressionProcessingDependencyException(
                    message: "Post Impression dependency error occurred, please contact support",
                    innerException: dependencyException.InnerException as Xeption);

            this.postImpressionServiceMock.Setup(service =>
                service.RetrieveAllPostImpressions()).Throws(dependencyException);

            // when
            ValueTask<PostImpression> upsertPostImpressionTask =
                this.postImpressionProcessingService.UpsertPostImpressionAsync(
                    somePostImpression);

            PostImpressionProcessingDependencyException
                actualPostImpressionProcessingDependencyException =
                await Assert.ThrowsAsync<PostImpressionProcessingDependencyException>(
                    upsertPostImpressionTask.AsTask);

            // then
            actualPostImpressionProcessingDependencyException.Should().BeEquivalentTo(
                expectedPostImpressionProcessingDependencyException);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionProcessingDependencyException))),
                    Times.Once);

            this.postImpressionServiceMock.Verify(service =>
                service.AddPostImpressions(
                    It.IsAny<PostImpression>()),
                    Times.Never);

            this.postImpressionServiceMock.Verify(service =>
                service.ModifyPostImpressionAsync(
                    It.IsAny<PostImpression>()),
                    Times.Never);

            this.postImpressionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnUpsertIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var somePostImpression = CreateRandomPostImpression();
            var serviceException = new Exception();

            var failedPostImpressionProcessingServiceException =
                new FailedPostImpressionProcessingServiceException(
                    message: "Failed Post Impression service occurred, please contact support",
                    innerException: serviceException);

            var expectedPostImpressionProcessingServiceException =
                new PostImpressionProcessingServiceException(
                    message: "Failed Post Impression external service occurred, please contact support",
                    innerException: failedPostImpressionProcessingServiceException);

            this.postImpressionServiceMock.Setup(service =>
                service.RetrieveAllPostImpressions()).Throws(serviceException);

            // when
            ValueTask<PostImpression> upsertPostImpressionTask =
                this.postImpressionProcessingService.UpsertPostImpressionAsync(
                    somePostImpression);

            PostImpressionProcessingServiceException
                actualPostImpressionProcessingServiceException =
                await Assert.ThrowsAsync<PostImpressionProcessingServiceException>(
                    upsertPostImpressionTask.AsTask);

            // then
            actualPostImpressionProcessingServiceException.Should().BeEquivalentTo(
                expectedPostImpressionProcessingServiceException);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionProcessingServiceException))),
                    Times.Once);

            this.postImpressionServiceMock.Verify(service =>
                service.AddPostImpressions(
                    It.IsAny<PostImpression>()),
                    Times.Never);

            this.postImpressionServiceMock.Verify(service =>
                service.ModifyPostImpressionAsync(
                    It.IsAny<PostImpression>()),
                    Times.Never);

            this.postImpressionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}