// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Groups.Exceptions
{
    public class FailedGroupServiceException : Xeption
    {
        public FailedGroupServiceException(Exception innerException)
            : base(
                message: "Failed group service error occurred, please contact support.",
                innerException: innerException)
        { }

        public FailedGroupServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}