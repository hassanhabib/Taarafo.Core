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
    }
}
