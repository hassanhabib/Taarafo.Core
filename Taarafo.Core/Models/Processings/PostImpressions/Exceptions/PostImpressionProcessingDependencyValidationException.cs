// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Processings.PostImpressions.Exceptions
{
    public class PostImpressionProcessingDependencyValidationException : Xeption
    {
        public PostImpressionProcessingDependencyValidationException(Xeption innerException)
            : base(message: "Country dependency validation error occurred, please try again.",
                innerException)
        { }
    }
}
