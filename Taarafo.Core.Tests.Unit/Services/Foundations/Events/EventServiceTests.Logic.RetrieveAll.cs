// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.Events;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Events
{
    public partial class EventServiceTests
    {
        [Fact]
        private void ShouldRetrieveAllEvents()
        {
            // given
            IQueryable<Event> randomEvents = CreateRandomEvents();
            IQueryable<Event> storageEvents = randomEvents;
            IQueryable<Event> expectedEvents = storageEvents;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllEvents()).Returns(storageEvents);

            // when
            IQueryable<Event> actualEvent =
                this.eventService.RetrieveAllEvents();

            // then
            actualEvent.Should().BeEquivalentTo(expectedEvents);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllEvents(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}