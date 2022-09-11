using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Brokers.DateTimes;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Events;

namespace Taarafo.Core.Services.Foundations.Events
{
    public partial class EventService : IEventService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public EventService(
            IDateTimeBroker dateTimeBroker, 
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.storageBroker = storageBroker;
        }

        public async ValueTask<Event> AddEventAsync(Event @event) => 
            await this.storageBroker.InsertEventAsync(@event);

        public IQueryable<Event> RetrieveAllEvents() =>
            this.storageBroker.SelectAllEvents();
    }
}