// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Posts.Exceptions
{
    public class PostDependencyValidationException : Xeption
    {
        public PostDependencyValidationException(Xeption message)
            : base("Post dependency validation occured, please try again.", message)
        { }
    }
}
