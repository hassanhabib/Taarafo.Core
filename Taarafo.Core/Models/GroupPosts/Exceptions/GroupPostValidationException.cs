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
            : base(message: "GroupPost validation errors occurred, please try again.",
                  innerException)
        { }
    }
}