// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Processings.PostImpressions.Exceptions
{
    public class PostImpressionProcessingDependencyException : Xeption
    {
        public PostImpressionProcessingDependencyException(Xeption innerException)
            : base(
                message: "Post Impression dependency error occurred, please contact support",
                innerException: innerException)
        { }

        public PostImpressionProcessingDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}