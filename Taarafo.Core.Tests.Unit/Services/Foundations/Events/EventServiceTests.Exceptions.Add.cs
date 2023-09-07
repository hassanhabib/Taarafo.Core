// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Taarafo.Core.Models.Events;
using Taarafo.Core.Models.Events.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Events
{
    public partial class EventServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnCreateIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Event someEvent = CreateRandomEvent(randomDateTime);
            
            SqlException sqlException = 
                GetSqlException();

            var failedEventStorageException =
                new FailedEventStorageException(
                    message: "Failed event storage error occured, contact support.",
                    innerException: sqlException);

            var expectedEventDependencyException =
                new EventDependencyException(
                    message: "Event dependency validation occurred, please try again.",
                    innerException: failedEventStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Event> addEventTask =
                this.eventService.AddEventAsync(someEvent);

            EventDependencyException actualEventDependencyException =
                 await Assert.ThrowsAsync<EventDependencyException>(
                    addEventTask.AsTask);

            // then
            actualEventDependencyException.Should().BeEquivalentTo(
                expectedEventDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedEventDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertEventAsync(It.IsAny<Event>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnAddIfEventAlreadyExsitsAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Event randomEvent = CreateRandomEvent(randomDateTimeOffset);
            Event alreadyExistsEvent = randomEvent;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsEventException =
                new AlreadyExistsEventException(
                    message: "Event with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedEventDependencyValidationException =
                new EventDependencyValidationException(
                    message: "Event dependency validation occurred, please try again.",
                    innerException: alreadyExistsEventException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<Event> addEventTask =
                this.eventService.AddEventAsync(alreadyExistsEvent);

            EventDependencyValidationException actualEventDependencyValidationException =
                 await Assert.ThrowsAsync<EventDependencyValidationException>(
                    addEventTask.AsTask);

            // then
            actualEventDependencyValidationException.Should().BeEquivalentTo(
                expectedEventDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertEventAsync(It.IsAny<Event>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEventDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async void ShouldThrowDependencyValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Event someEvent = CreateRandomEvent();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidEventReferenceException =
                new InvalidEventReferenceException(
                    message: "Invalid eventt reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var expectedEventDependencyValidationException =
                new EventDependencyValidationException(
                    message: "Event dependency validation occurred, please try again.",
                    innerException: invalidEventReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Event> addEventTask =
                this.eventService.AddEventAsync(someEvent);

            EventDependencyValidationException actualEventDependencyValidationException =
                 await Assert.ThrowsAsync<EventDependencyValidationException>(
                     addEventTask.AsTask);

            // then
            actualEventDependencyValidationException.Should().BeEquivalentTo(
                expectedEventDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEventDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertEventAsync(It.IsAny<Event>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Event someEvent = CreateRandomEvent();

            var databaseUpdateException =
                new DbUpdateException();

            var failedEventStorageException =
                new FailedEventStorageException(
                    message: "Failed event storage error occured, contact support.",
                    innerException: databaseUpdateException);

            var expectedEventDependencyException =
                new EventDependencyException(
                    message: "Event dependency validation occurred, please try again.",
                    innerException: failedEventStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Event> addEventTask =
                this.eventService.AddEventAsync(someEvent);

            EventDependencyException actualEventDependencyException =
                 await Assert.ThrowsAsync<EventDependencyException>(
                     addEventTask.AsTask);

            // then
            actualEventDependencyException.Should().BeEquivalentTo(
                expectedEventDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertEventAsync(It.IsAny<Event>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEventDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Event someEvent = CreateRandomEvent();
            var serviceException = new Exception();

            var failedEventServiceException =
                new FailedEventServiceException(
                    message: "Failed event service occurred, please contact support.",
                    innerException: serviceException);

            var expectedEventServiceException =
                new EventServiceException(
                    message: "Event service error occurred, contact support.",
                    innerException: failedEventServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Event> addEventTask =
                this.eventService.AddEventAsync(someEvent);

            EventServiceException actualEventServiceException =
                 await Assert.ThrowsAsync<EventServiceException>(
                     addEventTask.AsTask);

            // then
            actualEventServiceException.Should().BeEquivalentTo(
                expectedEventServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertEventAsync(It.IsAny<Event>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEventServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}