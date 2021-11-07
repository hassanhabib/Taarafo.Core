// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Comments.Exceptions
{
    public class CommentDependencyException : Xeption
    {
        public CommentDependencyException(Exception innerException) :
            base(message: "Comment dependency error occured, contact support.", innerException)
        { }
    }
}
