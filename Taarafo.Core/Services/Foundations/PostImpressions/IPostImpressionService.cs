// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Taarafo.Core.Models.PostImpressions;

namespace Taarafo.Core.Services.Foundations.PostImpressions
{
    public interface IPostImpressionService
    {
        ValueTask<PostImpression> AddPostImpressions(PostImpression postImpression);
        ValueTask<PostImpression> RemovePostImpressionByIdAsync(Guid postImpressionId);
    }
}
