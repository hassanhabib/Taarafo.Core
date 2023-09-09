//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions
{
    public class LockedGroupPostException : Xeption
    {
        public LockedGroupPostException(Exception innerException)
            : base(
                message: "GroupPost is locked, please try again.",
                innerException: innerException)
        { }

        public LockedGroupPostException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}