// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupMemberships.Exceptions
{
    public class NotFoundGroupMembershipException : Xeption
    {
        public NotFoundGroupMembershipException(Guid groupMembershipId)
            : base(message: $"Couldn't find GroupMembership with id: {groupMembershipId}.")
        { }
    }
}