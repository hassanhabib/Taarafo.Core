// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Posts.Exceptions
{
    public class AlreadyExistsPostException : Xeption
    {
        public AlreadyExistsPostException(Exception innerException)
            : base(message: "Post with the same id already exists.", innerException)
        { }
    }
}
