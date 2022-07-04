// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Taarafo.Core.Models.PostImpressions;

namespace Taarafo.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<PostImpression> InsertPostImpressionAsync(PostImpression postImpression);
        ValueTask<PostImpression> UpdatePostImpressionAsync(PostImpression postImpression);
        ValueTask<PostImpression> SelectPostImpressionByIdAsync(Guid postImpressionId);
        ValueTask<PostImpression> DeletePostImpressionAsync(PostImpression postImpression);
    }
}
