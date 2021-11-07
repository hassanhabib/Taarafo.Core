// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;

namespace Taarafo.Core.Models.Comments.Exceptions
{
    public class CommentServiceException : Exception
    {
        public CommentServiceException(Exception innerException)
            : base(message: "Comment service error occurred, contact support.", innerException) { }
    }
}
