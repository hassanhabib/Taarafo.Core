// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Posts.Exceptions
{
    public class PostValidationException : Xeption
    {
        public PostValidationException(Xeption innerException)
            : base(message: "Post validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
