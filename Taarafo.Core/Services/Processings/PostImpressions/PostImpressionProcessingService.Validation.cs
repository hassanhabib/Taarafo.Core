// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.Processings.PostImpressions.Exceptions;

namespace Taarafo.Core.Services.Processings.PostImpressions
{
    public partial class PostImpressionProcessingService
    {
        private static void ValidatePostImpression(PostImpression postImpression)
        {
            if (postImpression is null)
            {
                throw new NullPostImpressionProcessingException();
            }
        }
    }
}
