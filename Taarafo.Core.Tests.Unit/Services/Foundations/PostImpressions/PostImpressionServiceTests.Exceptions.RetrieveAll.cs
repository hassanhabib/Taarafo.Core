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

            var failedPostImpressionStorageException =
                new FailedPostImpressionStorageException(sqlException);

            var expectedPostImpressionDependencyException = new
                PostImpressionDependencyException(failedPostImpressionStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPostImpressions()).Throws(sqlException);

            //when
            Action retrieveAllPostImpressions = () =>
                this.postImpressionService.RetrieveAllPostImpressions();

            //then
            Assert.Throws<PostImpressionDependencyException>(
                retrieveAllPostImpressions);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectAllPostImpressions(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogCritical(It.Is(SameExceptionAs(
                  expectedPostImpressionDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            //given 
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedPostImpressionServiceException =
                new FailedPostImpressionServiceException(serviceException);

            var expectedPostImpressionServiceException =
                new PostImpressionServiceException(failedPostImpressionServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPostImpressions()).Throws(serviceException);

            //when
            Action retrieveAllPostImpressionsAction = () =>
                this.postImpressionService.RetrieveAllPostImpressions();

            //then
            Assert.Throws<PostImpressionServiceException>(
                retrieveAllPostImpressionsAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPostImpressions(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                   expectedPostImpressionServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
