// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.PostImpressions;

namespace Taarafo.Core.Services.Foundations.PostImpressions
{
    public interface IPostImpressionService
    {
        ValueTask<PostImpression> AddPostImpressions(PostImpression postImpression);
        IQueryable<PostImpression> RetrieveAllPostImpressions();
        ValueTask<PostImpression> RemovePostImpressionByIdAsync(Guid postId, Guid profileId);
    }
}
