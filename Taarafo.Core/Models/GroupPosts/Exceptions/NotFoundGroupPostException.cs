// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions;

public class NotFoundGroupPostException : Xeption
{
    public NotFoundGroupPostException(Guid groupPostId)
        : base(message: $"Couldn't find group post with id: {groupPostId}.")
    { }
}