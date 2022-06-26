// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.PostImpressions.Exceptions
{
    public class PostImpressionValidationException : Xeption
    {
        public PostImpressionValidationException(Xeption innerException)
            : base(message: "Post Impression validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
