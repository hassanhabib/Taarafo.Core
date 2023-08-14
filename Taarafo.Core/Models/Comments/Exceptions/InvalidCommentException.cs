// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Comments.Exceptions
{
    public class InvalidCommentException : Xeption
    {
        public InvalidCommentException()
            : base(
                message: "Invalid comment. Please correct the errors and try again.")
        { }

        public InvalidCommentException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}