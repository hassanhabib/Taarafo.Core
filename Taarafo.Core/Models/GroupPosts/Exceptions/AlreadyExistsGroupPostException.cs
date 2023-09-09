// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions
{
    public class AlreadyExistsGroupPostException : Xeption
    {
        public AlreadyExistsGroupPostException(Exception innerException)
            : base(
                message: "Group post with the same id already exists.",
                innerException: innerException)
        { }
        
        public AlreadyExistsGroupPostException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}