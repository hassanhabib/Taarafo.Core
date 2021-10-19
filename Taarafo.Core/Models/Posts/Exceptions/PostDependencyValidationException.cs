// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Posts.Exceptions
{
    public class PostDependencyValidationException : Xeption
    {
        public PostDependencyValidationException(Xeption innerException)
            : base(message: "Post dependency validation occured, please try again.", innerException)
        { }
    }
}
