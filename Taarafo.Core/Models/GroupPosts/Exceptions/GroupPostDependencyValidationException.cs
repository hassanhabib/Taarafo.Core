// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions
{
    public class GroupPostDependencyValidationException : Xeption
    {
        public GroupPostDependencyValidationException(Xeption innerException)
            : base(message: "Group post dependency validation occurred, please try again.",
                  innerException)
        { }
    }
}
