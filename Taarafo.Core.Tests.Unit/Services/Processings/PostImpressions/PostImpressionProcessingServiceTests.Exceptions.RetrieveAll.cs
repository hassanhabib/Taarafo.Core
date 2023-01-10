// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
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
        public void ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            //given
            var somePostImpressions = CreateRandomPostImpressions();

            var expectedPostImpressionProcessingDependencyException =
                new PostImpressionProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.postImpressionServiceMock.Setup(service =>
                service.RetrieveAllPostImpressions()).Throws(dependencyException);

            //when
            Action retrieveAllPostImpressionAction = () =>
                this.postImpressionProcessingService.RetrieveAllPostImpressionsAsync();

            //then
            Assert.Throws<PostImpressionProcessingDependencyException>(
                retrieveAllPostImpressionAction);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionProcessingDependencyException))), Times.Once);

            this.postImpressionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            //given
            var somePostImpression = CreateRandomPostImpressions();
            var serviceException = new Exception();

            var failedPostImpressionProcessingServiceException =
                new FailedPostImpressionProcessingServiceException(serviceException);

            var expectedPostImpressionProcessingServiceException =
                new PostImpressionProcessingServiceException(failedPostImpressionProcessingServiceException);

            this.postImpressionServiceMock.Setup(service =>
                service.RetrieveAllPostImpressions()).Throws(serviceException);

            //when
            Action retrieveAllPostImpressionAction = () =>
                this.postImpressionProcessingService.RetrieveAllPostImpressionsAsync();

            //then
            Assert.Throws<PostImpressionProcessingServiceException>(retrieveAllPostImpressionAction);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionProcessingServiceException))), Times.Once);

            this.postImpressionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
