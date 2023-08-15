// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Comments.Exceptions
{
    public class CommentServiceException : Xeption
    {
        public CommentServiceException(Exception innerException)
            : base(
                message: "Comment service error occurred, contact support.",
                    innerException: innerException)
        { }

        public CommentServiceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}