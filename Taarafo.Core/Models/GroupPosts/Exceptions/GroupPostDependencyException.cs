// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions;

public class GroupPostDependencyException : Xeption
{
    public GroupPostDependencyException(Xeption innerException)
        : base(message: "Group post dependency error occurred, contact support.", innerException)
    { }
}