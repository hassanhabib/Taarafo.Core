// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupMemberships.Exceptions
{
    public class GroupMembershipServiceException : Xeption
    {
        public GroupMembershipServiceException(Exception innerException)
            : base(
                message: "GroupMembership service error occurred, please contact support.",
                innerException: innerException)
        { }
        
        public GroupMembershipServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}