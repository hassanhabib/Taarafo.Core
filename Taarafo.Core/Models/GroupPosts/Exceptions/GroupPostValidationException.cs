// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions
{
    public class GroupPostValidationException : Xeption
    {
        public GroupPostValidationException(Xeption innerException)
            : base (message: "Group post validation error occurred, please try again.",
                  innerException)
        { }
    }
}
