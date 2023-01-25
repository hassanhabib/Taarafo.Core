// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupMemberships.Exceptions
{
    public class FailedGroupMembershipStorageException : Xeption
    {
        public FailedGroupMembershipStorageException(Exception innerException)
            : base(message: "Failed GroupMembership storage error occured, contact support.", innerException)
        { }
    }
}