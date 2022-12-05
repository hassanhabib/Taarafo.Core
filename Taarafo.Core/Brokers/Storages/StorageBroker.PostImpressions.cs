// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Taarafo.Core.Models.PostImpressions;

namespace Taarafo.Core.Brokers.Storages
{
	public partial class StorageBroker
	{
		public DbSet<PostImpression> PostImpressions { get; set; }

		public async ValueTask<PostImpression> InsertPostImpressionAsync(PostImpression postImpression)
		{
			var broker =
				new StorageBroker(this.configuration);

			EntityEntry<PostImpression> postImpressionEntityEntry =
				await broker.PostImpressions.AddAsync(postImpression);

			await broker.SaveChangesAsync();

			return postImpressionEntityEntry.Entity;
		}

		public IQueryable<PostImpression> SelectAllPostImpressions()
		{
			using var broker =
				new StorageBroker(this.configuration);

			return broker.PostImpressions;
		}

		public async ValueTask<PostImpression> SelectPostImpressionByIdsAsync(Guid PostId, Guid ProfileId)
		{
			var broker =
				new StorageBroker(this.configuration);

			return await broker.PostImpressions.FindAsync(PostId, ProfileId);
		}

		public async ValueTask<PostImpression> UpdatePostImpressionAsync(PostImpression postImpression)
		{
			using var broker =
				new StorageBroker(this.configuration);

			EntityEntry<PostImpression> postImpressionEntityEntry =
				broker.PostImpressions.Update(postImpression);

			await broker.SaveChangesAsync();

			return postImpressionEntityEntry.Entity;
		}

		public async ValueTask<PostImpression> DeletePostImpressionAsync(PostImpression postImpression)
		{
			using var broker =
				new StorageBroker(this.configuration);

			EntityEntry<PostImpression> postImpressionEntityEntry =
				broker.PostImpressions.Remove(postImpression);

			await broker.SaveChangesAsync();

			return postImpressionEntityEntry.Entity;
		}
	}
}
