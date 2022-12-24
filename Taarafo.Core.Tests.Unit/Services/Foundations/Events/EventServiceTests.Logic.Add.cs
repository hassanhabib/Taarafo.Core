// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.Events;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Events
{
    public partial class EventServiceTests
    {
        [Fact]
        public async Task ShouldAddEventAsync()
        {
            //given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            Event randomEvent = CreateRandomEvent();
            Event inputEvent = randomEvent;
            Event storageEvent = inputEvent;
            Event expectedEvent = storageEvent.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertEventAsync(inputEvent))
                    .ReturnsAsync(storageEvent);

            //when
            Event actualEvent = await this.eventService.AddEventAsync(inputEvent);

            //then
            actualEvent.Should().BeEquivalentTo(expectedEvent);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertEventAsync(inputEvent), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
