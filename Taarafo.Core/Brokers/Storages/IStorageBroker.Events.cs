// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Events;

namespace Taarafo.Core.Brokers.Storages
{
	public partial interface IStorageBroker
	{
		ValueTask<Event> InsertEventAsync(Event @event);
		IQueryable<Event> SelectAllEvents();
		ValueTask<Event> SelectEventByIdAsync(Guid eventId);
		ValueTask<Event> DeleteEventAsync(Event @event);
	}
}
