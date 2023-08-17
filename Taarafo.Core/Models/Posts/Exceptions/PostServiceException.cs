// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Posts.Exceptions
{
    public class PostServiceException : Xeption
    {
        public PostServiceException(Exception innerException)
            : base(
                message: "Post service error occurred, contact support.",
                    innerException: innerException) { }

        public PostServiceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}