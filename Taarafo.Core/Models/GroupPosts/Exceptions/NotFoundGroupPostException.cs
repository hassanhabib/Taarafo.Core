// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions
{
    public class NotFoundGroupPostException : Xeption
    {
        public NotFoundGroupPostException(Guid groupPostId)
          : base(message: $"Could not find the grouppost with id: {groupPostId}.")
        { }
    }
}