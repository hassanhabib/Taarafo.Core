// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.PostImpressions.Exceptions
{
    public class PostImpressionDependencyValidationException : Xeption
    {
        public PostImpressionDependencyValidationException(Xeption innerException)
            : base(message: "Post Impression dependency validation occurred, please try again.", innerException)
        { }
    }
}
