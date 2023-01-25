// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.GroupMemberships.Exceptions
{
    public class GroupMembershipDependencyException : Xeption
    {
        public GroupMembershipDependencyException(Xeption innerException)
           : base(message: "GroupMembership dependency validation occurred, please try again.",
                 innerException)
        { }
    }
}