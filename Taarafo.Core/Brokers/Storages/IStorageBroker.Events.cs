using Taarafo.Core.Models.Events;
using System.Threading.Tasks;

namespace Taarafo.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Event> InsertEventAsynce(Event @event);
    }
}
