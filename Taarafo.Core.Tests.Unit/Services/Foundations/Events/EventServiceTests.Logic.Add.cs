﻿// ---------------------------------------------------------------
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
        private async Task ShouldAddEventAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Event randomEvent = CreateRandomEvent(randomDateTime);
            Event inputEvent = randomEvent;
            Event insertedEvent = inputEvent;
            Event expectedEvent = insertedEvent.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertEventAsync(inputEvent))
                    .ReturnsAsync(insertedEvent);

            // when
            Event actualEvent =
                await this.eventService.AddEventAsync(inputEvent);

            // then
            actualEvent.Should().BeEquivalentTo(expectedEvent);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertEventAsync(inputEvent),
                    Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}