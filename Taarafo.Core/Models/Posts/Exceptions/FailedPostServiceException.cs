// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Posts.Exceptions
{
    public class FailedPostServiceException : Xeption
    {
        public FailedPostServiceException(Exception innerException)
            : base("Failed post service occured, please contact support", innerException)
        {}
    }
}
