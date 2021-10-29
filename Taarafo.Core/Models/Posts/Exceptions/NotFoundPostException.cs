// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Posts.Exceptions
{
    public class NotFoundPostException : Xeption
    {
        public NotFoundPostException(Guid id)
            : base($"Could not find post with id: {id}")
        { }
    }
}
