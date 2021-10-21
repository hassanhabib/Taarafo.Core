// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;

namespace Taarafo.Core.Models.Posts.Exceptions
{
    public class PostValidationException : Exception
    {
        public PostValidationException(Exception innerException)
            : base(message: "Post validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
