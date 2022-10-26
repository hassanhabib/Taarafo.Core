// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions
{
    public class FailedGroupPostStorageException : Xeption        
    {
        public FailedGroupPostStorageException(Exception innerException)
            : base(message: "Failed group post storage error occured, contact support.", innerException)
        { }       
    }
}
