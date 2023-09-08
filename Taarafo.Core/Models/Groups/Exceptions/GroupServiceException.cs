// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Groups.Exceptions
{
    public class GroupServiceException : Xeption
    {
        public GroupServiceException(Xeption innerException)
            : base(
                message: "Group service error occurred, contact support.",
                innerException: innerException)
        { }

        public GroupServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}