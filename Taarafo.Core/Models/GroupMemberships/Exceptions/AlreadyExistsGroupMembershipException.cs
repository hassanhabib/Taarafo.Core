// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupMemberships.Exceptions
{
    public class AlreadyExistsGroupMembershipException : Xeption
    {
        public AlreadyExistsGroupMembershipException(Exception innerException)
            : base(message: "GroupMembership with the same id already exists.", innerException)
        { }
    }
}