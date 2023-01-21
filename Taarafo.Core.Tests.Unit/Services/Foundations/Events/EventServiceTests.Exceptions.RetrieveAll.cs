// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.Events.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Events
{
    public partial class EventServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = CreateSqlException();
            var failedEventStorageException =
                new FailedEventStorageException(sqlException);

            var expectedEventDependencyException =
                new EventDependencyException(failedEventStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllEvents()).Throws(sqlException);

            // when
            Action retrieveAllEventAction = () =>
                this.eventService.RetrieveAllEvents();

            EventDependencyException actualEventDependencyException =
                Assert.Throws<EventDependencyException>(retrieveAllEventAction);

            // then
            actualEventDependencyException.Should().BeEquivalentTo(expectedEventDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllEvents(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedEventDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedEventServiceException = new FailedEventServiceException(serviceException);

            var expectedEventServiceException = new
                EventServiceException(failedEventServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllEvents()).Throws(serviceException);

            // when
            Action retrieveAllEventsAction = () =>
                this.eventService.RetrieveAllEvents();

            EventServiceException actualEventServiceException =
                Assert.Throws<EventServiceException>(
                    retrieveAllEventsAction);

            // then
            actualEventServiceException.Should().BeEquivalentTo(expectedEventServiceException);

            this.storageBrokerMock.Verify(broker => broker.SelectAllEvents(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedEventServiceException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
