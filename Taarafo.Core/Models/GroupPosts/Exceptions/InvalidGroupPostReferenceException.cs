// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions
{
    public class InvalidGroupPostReferenceException : Xeption
    {
        public InvalidGroupPostReferenceException(Exception innerException)
            : base(message: "Invalid group post reference error occurred.", innerException)
        { }
    }
}
