// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Comments.Exceptions
{
    public class ForeignKeyCommentReferenceException : Xeption
    {
        public ForeignKeyCommentReferenceException(Exception innerException)
            : base(
                  message: "ForeignKey exception has occurred, contact support.",
                    innerException: innerException) { }

        public ForeignKeyCommentReferenceException(string message, Exception innerException)
            : base(message, innerException) { }
        }
    }