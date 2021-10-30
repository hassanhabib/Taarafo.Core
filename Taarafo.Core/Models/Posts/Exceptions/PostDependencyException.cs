// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Posts.Exceptions
{
    public class PostDependencyException : Xeption
    {
        public PostDependencyException(Xeption innerException) :
            base(message: "Post dependency error occurred, contact support.", innerException)
        { }
    }
}
