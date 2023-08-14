// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Comments.Exceptions
{
    public class NotFoundCommentException : Xeption
    {
        public NotFoundCommentException(Guid commentId)
            : base(
                message: $"Comment with id: {commentId} not found, please contact support.")
        { }

        public NotFoundCommentException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}