using System.Threading.Tasks;
// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using Taarafo.Core.Models.Events;

namespace Taarafo.Core.Services.Foundations.Events
{
    public interface IEventService
    {
        ValueTask<Event> AddEventAsync(Event @event);
        IQueryable<Event> RetrieveAllEvents();
    }
}
