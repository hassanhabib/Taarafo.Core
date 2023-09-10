// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Processings.PostImpressions.Exceptions
{
    public class PostImpressionProcessingValidationException : Xeption
    {
        public PostImpressionProcessingValidationException(Xeption innerException)
            : base(
                message: "Post Impression validation error occurred, please try again.",
                innerException: innerException)
        { }

        public PostImpressionProcessingValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}