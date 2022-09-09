using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Events;

namespace Taarafo.Core.Services.Foundations.Events
{
    public interface IEventService
    {
        IQueryable<Event> RetrieveAllEvents();
        ValueTask<Event> AddEvenAsync(Event @event);
    }
}