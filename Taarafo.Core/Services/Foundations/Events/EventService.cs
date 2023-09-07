// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Event> AddEventAsync(Event @event) =>
        TryCatch(async () =>
            {
                ValidateEventOnAdd(@event);

                return await this.storageBroker.InsertEventAsync(@event);

            });

        public IQueryable<Event> RetrieveAllEvents() =>
            TryCatch(() => this.storageBroker.SelectAllEvents());
    }
}