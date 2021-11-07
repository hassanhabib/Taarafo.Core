// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Comments.Exceptions
{
    public class FailedCommentServiceException : Xeption
    {
        public FailedCommentServiceException(Exception innerException)
            : base(message: "Failed comment service occurred, please contact support", innerException)
        { }
    }
}
