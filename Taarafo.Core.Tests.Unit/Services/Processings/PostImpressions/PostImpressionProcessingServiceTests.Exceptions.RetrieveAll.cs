// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.Processings.PostImpressions.Exceptions;
using Xeptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Processings.PostImpressions
{
    public partial class PostImpressionProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        private void ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var somePostImpressions = CreateRandomPostImpressions();

            var expectedPostImpressionProcessingDependencyException =
                new PostImpressionProcessingDependencyException(
                    message: "Post Impression dependency error occurred, please contact support",
                    innerException: dependencyException.InnerException as Xeption);

            this.postImpressionServiceMock.Setup(service =>
                service.RetrieveAllPostImpressions()).Throws(dependencyException);

            // when
            Action retrieveAllPostImpressionAction = () =>
                this.postImpressionProcessingService.RetrieveAllPostImpressions();

            PostImpressionProcessingDependencyException actualPostImpressionProcessingDependencyException =
                Assert.Throws<PostImpressionProcessingDependencyException>(retrieveAllPostImpressionAction);

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

            this.postImpressionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var somePostImpression = CreateRandomPostImpressions();
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
            Action retrieveAllPostImpressionAction = () =>
                this.postImpressionProcessingService.RetrieveAllPostImpressions();

            PostImpressionProcessingServiceException actualPostImpressionProcessingDependencyException =
                Assert.Throws<PostImpressionProcessingServiceException>(retrieveAllPostImpressionAction);

            // then
            actualPostImpressionProcessingDependencyException.Should().BeEquivalentTo(
                expectedPostImpressionProcessingServiceException);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionProcessingServiceException))),
                    Times.Once);

            this.postImpressionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}