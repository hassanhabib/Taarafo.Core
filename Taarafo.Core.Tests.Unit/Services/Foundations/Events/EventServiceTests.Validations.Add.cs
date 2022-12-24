// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            //given
            Event noEvent = null;
            var nullEventException = new NullEventException();

            var expectedEventValidationException = 
                new EventValidationException(nullEventException);

            //when
            ValueTask<Event> AddEventTask = this.eventService.AddEventAsync(noEvent);

            EventValidationException actualEventValidationException = 
                await Assert.ThrowsAsync<EventValidationException>(AddEventTask.AsTask);

            //then
            actualEventValidationException.Should().BeEquivalentTo(
                expectedEventValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEventValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertEventAsync(It.IsAny<Event>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfEventIsInvalidAndLogItAsync(
            string invalidString)
        {
            //given
            Event invalidEvent = new Event
            {
                Name = invalidString,
            };

            var invalidEventException = new InvalidEventException();

            invalidEventException.AddData(
                key: nameof(Event.Id),
                values: "Id is required"); 
            
            invalidEventException.AddData(
                key: nameof(Event.Name),
                values: "Text is required");

            invalidEventException.AddData(
              key: nameof(Event.Date),
              values: "Date is required");

            invalidEventException.AddData(
              key: nameof(Event.Location),
              values: "Location is required");

            invalidEventException.AddData(
                key: nameof(Event.Image),
                values: "Text is required");

            invalidEventException.AddData(
              key: nameof(Event.CreatedBy),
              values: "Id is required");

            invalidEventException.AddData(
              key: nameof(Event.CreatedDate),
              values: "Date is required");

            invalidEventException.AddData(
              key: nameof(Event.UpdatedBy),
              values: "Id is required");

            invalidEventException.AddData(
              key: nameof(Event.UpdatedDate),
              values: "Date is required");

            EventValidationException expectedEventValidationException =
                new EventValidationException(invalidEventException);

            //when
            ValueTask<Event> addEventTask = 
                this.eventService.AddEventAsync(invalidEvent);

            EventValidationException actualEventValidationException =
                await Assert.ThrowsAsync<EventValidationException>(addEventTask.AsTask);

            //then
            actualEventValidationException.Should().BeEquivalentTo(
                expectedEventValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEventValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertEventAsync(It.IsAny<Event>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
