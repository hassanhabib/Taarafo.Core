// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions
{
    public class NullGroupPostException : Xeption
    {
        public NullGroupPostException()
            : base(message: "Group post is null.")
        { }
    }
}
