// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.PostImpressions.Exceptions
{
    public class PostImpressionDependencyException : Xeption
    {
        public PostImpressionDependencyException(Xeption innerException)
            : base(
                message: "Post impression dependency error has occurred, please contact support.",
                innerException: innerException)
        { }

        public PostImpressionDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}