// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Taarafo.Core.Models.Posts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostImpressions
{
    public partial class PostImpressionServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            PostImpression randomPostImpression = CreateRandomModifyPostImpression(randomDateTime);
            PostImpression somePostImpression = randomPostImpression;
            Guid postId = somePostImpression.PostId;
            Guid profileId = somePostImpression.ProfileId;
            SqlException sqlException = GetSqlException();

            var failedPostImpressionStorageException =
                new FailedPostImpressionStorageException(sqlException);

            var expectedPostImpressionDependencyException =
                new PostImpressionDependencyException(failedPostImpressionStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdsAsync(postId, profileId)).Throws(sqlException);

            //when
            ValueTask<PostImpression> modifyPostImpression =
                this.postImpressionService.ModifyPostImpressionAsync(somePostImpression);

            PostImpressionDependencyException actualPostImpressionDependencyException =
                await Assert.ThrowsAsync<PostImpressionDependencyException>(modifyPostImpression.AsTask);

            //then
            actualPostImpressionDependencyException.Should().BeEquivalentTo(expectedPostImpressionDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedPostImpressionDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdsAsync(postId, profileId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePostImpressionAsync(somePostImpression), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            //given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            PostImpression randomPostImpression = CreateRandomPostImpression(randomDateTime);
            PostImpression somePostIpression = randomPostImpression;
            Guid postId = somePostIpression.PostId;
            Guid profileId= somePostIpression.ProfileId;
            somePostIpression.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedPostImpressionStorageException = 
                new FailedPostImpressionStorageException(databaseUpdateException);

            var expectedPostImpressionDependencyException = 
                new PostImpressionDependencyException(failedPostImpressionStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdsAsync(postId, profileId)).ThrowsAsync(databaseUpdateException);

            //when
            ValueTask<PostImpression> modifyPostImpression =
                this.postImpressionService.ModifyPostImpressionAsync(somePostIpression);

            PostImpressionDependencyException actualPostImpressionDependencyException =
                await Assert.ThrowsAsync<PostImpressionDependencyException>(modifyPostImpression.AsTask);

            //then
            actualPostImpressionDependencyException.Should().BeEquivalentTo(
                expectedPostImpressionDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdsAsync(postId, profileId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPostImpressionDependencyException))), Times.Once);


            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            //given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            PostImpression randomPostImpression = CreateRandomPostImpression(randomDateTime);
            PostImpression somePostImpression = randomPostImpression;
            randomPostImpression.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            Guid postId = somePostImpression.PostId;
            Guid profileId= somePostImpression.ProfileId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedPostImpressionException = 
                new LockedPostImpressionException(databaseUpdateConcurrencyException);

            var expectedPostImpressionDependencyValidationException =
                new PostImpressionDependencyValidationException(lockedPostImpressionException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdsAsync(postId, profileId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            //when
            ValueTask<PostImpression> modifyPostImpression =
                this.postImpressionService.ModifyPostImpressionAsync(somePostImpression);

            PostImpressionDependencyValidationException actualPostImpressionDependencyValidationException =
                await Assert.ThrowsAsync<PostImpressionDependencyValidationException>(modifyPostImpression.AsTask);

            //then
            actualPostImpressionDependencyValidationException.Should().BeEquivalentTo(
                expectedPostImpressionDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdsAsync(postId, profileId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionDependencyValidationException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
