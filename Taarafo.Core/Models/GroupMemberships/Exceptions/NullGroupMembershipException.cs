// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.GroupMemberships.Exceptions
{
    public class NullGroupMembershipException : Xeption
    {
        public NullGroupMembershipException()
            : base(message: "GroupMembership is null.")
        { }
        
        public NullGroupMembershipException(string message)
            : base(message)
        { }
    }
}