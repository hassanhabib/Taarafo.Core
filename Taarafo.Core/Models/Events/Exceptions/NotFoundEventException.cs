// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Events.Exceptions
{
    public class NotFoundEventException : Xeption
    {
        public NotFoundEventException(Guid groupId, Guid postId)
            : base(message: $"Couldn't find event with id: {groupId}, {postId}.")
        { }
    }
}
