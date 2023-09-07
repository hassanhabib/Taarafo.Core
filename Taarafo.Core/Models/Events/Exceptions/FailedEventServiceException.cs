// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Events.Exceptions
{
    public class FailedEventServiceException : Xeption
    {
        public FailedEventServiceException(Exception innerException)
            : base(
                message: "Failed event service occurred, please contact support.",
                innerException: innerException)
        { }

        public FailedEventServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}