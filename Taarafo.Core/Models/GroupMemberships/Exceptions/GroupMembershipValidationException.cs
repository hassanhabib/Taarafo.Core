// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.GroupMemberships.Exceptions
{
    public class GroupMembershipValidationException : Xeption
    {
        public GroupMembershipValidationException(Xeption innerException)
            : base(message: "GroupMembership validation error occurred, please try again.",
                  innerException)
        { }
    }
}