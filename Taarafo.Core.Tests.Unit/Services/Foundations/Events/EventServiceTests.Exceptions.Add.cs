// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.Events;
using Taarafo.Core.Models.Events.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Events
{
    public partial class EventServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogIsAsync()
        {
            //given
            Event someEvent = CreateRandomEvent();
            SqlException sqlException = CreateSqlException();

            var failedEventStorageException = new FailedEventStorageException(sqlException);

            var expectedEventDependencyException =
                new EventDependencyException(failedEventStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertEventAsync(It.IsAny<Event>()))
                    .ThrowsAsync(sqlException);
                    
            //when
            ValueTask<Event> addEventTask = this.eventService.AddEventAsync(someEvent);


            EventDependencyException actualEventDependencyException =
                await Assert.ThrowsAsync<EventDependencyException>(addEventTask.AsTask);

            //then
            actualEventDependencyException.Should().BeEquivalentTo(expectedEventDependencyException);

            this.storageBrokerMock.Verify(broker => 
                broker.InsertEventAsync(It.IsAny<Event>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedEventDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            //given
            Event someEvent = CreateRandomEvent();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsEventException = new AlreadyExistsEventException(duplicateKeyException);

            var expectedDependencyValidationException =
               new EventDependencyValidationException(alreadyExistsEventException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertEventAsync(It.IsAny<Event>()))
                    .ThrowsAsync(duplicateKeyException);

            //when
            ValueTask<Event> addEventTask = this.eventService.AddEventAsync(someEvent);

            EventDependencyValidationException actualEvenDependencyValidationException =
                await Assert.ThrowsAsync<EventDependencyValidationException>(addEventTask.AsTask);

            //then
            actualEvenDependencyValidationException.Should()
                .BeEquivalentTo(expectedDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertEventAsync(It.IsAny<Event>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
