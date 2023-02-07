// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.PostImpressions;

namespace Taarafo.Core.Services.Processings.PostImpressions
{
    public interface IPostImpressionProcessingService
    {
        ValueTask<PostImpression> UpsertPostImpressionAsync(PostImpression postImpression);
        IQueryable<PostImpression> RetrieveAllPostImpressions();

    }
}