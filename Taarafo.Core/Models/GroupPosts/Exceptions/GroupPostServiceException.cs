// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions
{
    public class GroupPostServiceException : Xeption
    {
        public GroupPostServiceException(Xeption innerException)
            : base(message: "Group post service error occurred, please contact support.", innerException)
        { }
    }
}
