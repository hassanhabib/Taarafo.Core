// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Taarafo.Core.Models.Events;

namespace Taarafo.Core.Brokers.Storages
{
	public partial class StorageBroker
	{
		public DbSet<Event> Events { get; set; }

		public async ValueTask<Event> InsertEventAsync(Event @event) =>
			await InsertEventAsync(@event);

		public IQueryable<Event> SelectAllEvents() =>
			SelectAll<Event>();

		public async ValueTask<Event> SelectEventByIdAsync(Guid eventId) =>
			await SelectAsync<Event>(eventId);

		public async ValueTask<Event> UpdateEventAsync(Event @event) =>
			await UpdateAsync(@event);
    		
        public async ValueTask<Event> DeleteEventAsync(Event @event) =>
			await DeleteAsync(@event);  
	}
}
