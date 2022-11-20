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

		public async ValueTask<Event> InsertEventAsync(Event @event)
		{
			using var broker =
				new StorageBroker(this.configuration);

			EntityEntry<Event> eventEntityEntry =
				await broker.Events.AddAsync(@event);

			await broker.SaveChangesAsync();

			return eventEntityEntry.Entity;
		}

		public IQueryable<Event> SelectAllEvents()
		{
			using var broker =
				new StorageBroker(this.configuration);

			return broker.Events;
		}

		public async ValueTask<Event> SelectEventByIdAsync(Guid eventId)
		{
			using var broker =
				new StorageBroker(this.configuration);

			return await broker.Events.FindAsync(eventId);
		}
		public async ValueTask<Event> UpdateEventAsync(Event @event) =>
			await UpdateAsync(@event);
	}
}
