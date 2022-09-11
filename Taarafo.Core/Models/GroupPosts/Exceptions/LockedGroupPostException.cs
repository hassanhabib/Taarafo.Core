// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions;

public class LockedGroupPostException : Xeption
{
    public LockedGroupPostException(Exception innerException)
        : base(message: "Locked group post record exception, please try again later", innerException)
    { }
}