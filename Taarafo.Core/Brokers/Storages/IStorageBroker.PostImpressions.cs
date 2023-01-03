// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.PostImpressions;

namespace Taarafo.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<PostImpression> InsertPostImpressionAsync(PostImpression postImpression);
        IQueryable<PostImpression> SelectAllPostImpressions();
        ValueTask<PostImpression> SelectPostImpressionByIdAsync(Guid postId, Guid profileId);
        ValueTask<PostImpression> UpdatePostImpressionAsync(PostImpression postImpression);
        ValueTask<PostImpression> DeletePostImpressionAsync(PostImpression postImpression);
    }
}
