// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Posts.Exceptions
{
    public class AlreadyExsitPostException : Xeption
    {
        public AlreadyExsitPostException(Exception innerException) :
            base("Post with the same id already exists.", innerException)
        { }
    }
}
