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

            var expectedPostDependencyException =
                new PostImpressionDependencyException(failedPostImpressionStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdsAsync(postId, profileId)).Throws(sqlException);

            //when
            ValueTask<PostImpression> modifyPostImpression =
                this.postImpressionService.ModifyPostImpressionAsync(somePostImpression);

            PostImpressionDependencyException actualPostDependencyException =
                await Assert.ThrowsAsync<PostImpressionDependencyException>(modifyPostImpression.AsTask);

            //then
            actualPostDependencyException.Should().BeEquivalentTo(actualPostDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedPostDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdsAsync(postId, profileId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePostImpressionAsync(somePostImpression), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
