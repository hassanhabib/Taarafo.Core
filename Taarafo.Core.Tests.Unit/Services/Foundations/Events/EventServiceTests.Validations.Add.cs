// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.Events;
using Taarafo.Core.Models.Events.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Events
{
    public partial class EventServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfEventIsNullAndLogitAsync()
        {
             // given
            Event nullEvent = null;

            var nullEventException =
                new NullEventException();

            var expectedEventValidationException =
                new EventValidationException(nullEventException);

            // when
            ValueTask<Event> addEventTask =
                this.eventService.AddEventAsync(nullEvent);

            EventValidationException actualEventValidationException =
                await Assert.ThrowsAsync<EventValidationException>(
                    addEventTask.AsTask);

            // then
            actualEventValidationException.Should().BeEquivalentTo(
                expectedEventValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEventValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfEventIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidGuid = Guid.Empty;

            var invalidEvent = new Event
            {
                Id = invalidGuid,
                CreatedBy = invalidGuid
            };

            var invalidEventException =
                new InvalidEventException();

            invalidEventException.AddData(
                key: nameof(Event.Id),
                values: "Id is required");

            invalidEventException.AddData(
                key: nameof(Event.Location),
                values: "Text is required");

            invalidEventException.AddData(
                key: nameof(Event.Date),
                values: "Date is required");

            invalidEventException.AddData(
                key: nameof(Event.CreatedDate),
                values: "Date is required");

            invalidEventException.AddData(
                key: nameof(Event.CreatedBy),
                values: "Id is required");

            var expectedEventValidationException =
                new EventValidationException(invalidEventException);

            //when
            ValueTask<Event> addEventTask =
                this.eventService.AddEventAsync(invalidEvent);

            EventValidationException actualEventValidationException =
                await Assert.ThrowsAsync<EventValidationException>(
                    addEventTask.AsTask);

            //then
            actualEventValidationException.Should().BeEquivalentTo(
                expectedEventValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEventValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertEventAsync(invalidEvent),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
