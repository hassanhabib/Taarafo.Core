// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Groups.Exceptions
{
    public class NotFoundGroupException : Xeption
    {
        public NotFoundGroupException(Guid groupId)
                : base(message: $"Couldn't find group with id: {groupId}.")
        { }
    }
}