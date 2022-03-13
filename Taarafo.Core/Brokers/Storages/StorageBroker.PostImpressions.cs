﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<PostImpression> postImpressionEntityEntry =
                await broker.PostImpressions.AddAsync(postImpression);

            await broker.SaveChangesAsync();
            return postImpressionEntityEntry.Entity;      
        }

    }
}
