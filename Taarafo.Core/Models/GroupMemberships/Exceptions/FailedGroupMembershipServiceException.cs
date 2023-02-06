// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupMemberships.Exceptions
{
    public class FailedGroupMembershipServiceException : Xeption
    {
        public FailedGroupMembershipServiceException(Exception innerException)
            : base(message: "Failed GroupMembership service occurred, please contact support.", innerException)
        { }
    }
}