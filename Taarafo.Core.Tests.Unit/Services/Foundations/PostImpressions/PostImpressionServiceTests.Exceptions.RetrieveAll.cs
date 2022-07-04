// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostImpressions
{
    public partial class PostImpressionServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            //given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedPostImpressionStorageException(sqlException);

            var expectedPostImpressionDependencyException =
                new PostImpressionDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPostImpressions())
                    .Throws(sqlException);

            //when
            Action retrieveAllPostImpressionsAction = () =>
                this.postImpressionService.RetrieveAllPostImpressions();

            //then
            Assert.Throws<PostImpressionDependencyException>(
                retrieveAllPostImpressionsAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPostImpressions(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostImpressionDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
