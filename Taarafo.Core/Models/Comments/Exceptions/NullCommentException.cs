// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Comments.Exceptions
{
    public class NullCommentException : Xeption
    {
        public NullCommentException()
            : base(message: "Comment is null, please correct.")
        { }

        public NullCommentException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}