// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions;

public class FailedGroupPostServiceException : Xeption
{
    public FailedGroupPostServiceException(Exception innerException)
        : base(message: "Failed group post service error occurred, please contact support.", innerException)
    { }
}