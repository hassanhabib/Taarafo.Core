// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.PostImpressions;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<PostImpression> PostImpressions { get; set; }

        public async ValueTask<PostImpression> InsertPostImpressionAsync(PostImpression postImpression) =>
            await InsertAsync(postImpression);

        public IQueryable<PostImpression> SelectAllPostImpressions() =>
            SelectAll<PostImpression>();

        public async ValueTask<PostImpression> SelectPostImpressionAsync(PostImpression postImpression) =>
            await SelectAsync<PostImpression>(postImpression);

        public async ValueTask<PostImpression> UpdatePostImpressionAsync(PostImpression postImpression) =>
            await UpdateAsync(postImpression);

        public async ValueTask<PostImpression> DeletePostImpressionAsync(PostImpression postImpression) =>
            await DeleteAsync(postImpression);
    }
}
