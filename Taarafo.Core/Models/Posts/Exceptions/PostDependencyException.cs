// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;

namespace Taarafo.Core.Models.Posts.Exceptions
{
    public class PostDependencyException : Exception
    {
        public PostDependencyException(Exception innerException) :
            base(message:"Post dependency error occured, contact support.", innerException)
        { }
    }
}
