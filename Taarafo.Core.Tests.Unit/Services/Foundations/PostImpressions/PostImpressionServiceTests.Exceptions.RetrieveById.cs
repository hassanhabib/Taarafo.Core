// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostImpressions
{
    public partial class PostImpressionServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid somePostId = Guid.NewGuid();
            Guid someProfileId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedPostImpressionStorageException =
                new FailedPostImpressionStorageException(sqlException);

            var expectedPostImpressionDependencyException =
                new PostImpressionDependencyException(failedPostImpressionStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(somePostId, someProfileId))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<PostImpression> retrievePostImpressionByIdTask =
                this.postImpressionService.RetrievePostImpressionByIdAsync(somePostId, someProfileId);

            PostImpressionDependencyException actualPostImpressionDependencyException =
                await Assert.ThrowsAsync<PostImpressionDependencyException>(
                    retrievePostImpressionByIdTask.AsTask);

            //then
            actualPostImpressionDependencyException.Should().BeEquivalentTo(
                   expectedPostImpressionDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(It.IsAny<Guid>(), (It.IsAny<Guid>())),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostImpressionDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid somePostId = Guid.NewGuid();
            Guid someProfileId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedPostImpressionServiceException =
                new FailedPostImpressionServiceException(serviceException);

            var expectedPostImpressionServiceException =
                new PostImpressionServiceException(failedPostImpressionServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(somePostId, someProfileId))
                    .ThrowsAsync(serviceException);

            //when
            ValueTask<PostImpression> retrievePostImpressionByIdTask =
                this.postImpressionService.RetrievePostImpressionByIdAsync(somePostId, someProfileId);

            PostImpressionServiceException actualPostImpressionServiceException =
                 await Assert.ThrowsAsync<PostImpressionServiceException>(retrievePostImpressionByIdTask.AsTask);

            //then
            actualPostImpressionServiceException.Should().BeEquivalentTo(expectedPostImpressionServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(It.IsAny<Guid>(), (It.IsAny<Guid>())),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}