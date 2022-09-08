using Taarafo.Core.Models.Events;
using System.Threading.Tasks;
using System.Linq;

namespace Taarafo.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Event> InsertEventAsync(Event @event);
        IQueryable<Event> SelectAllEvents();
    }
}
