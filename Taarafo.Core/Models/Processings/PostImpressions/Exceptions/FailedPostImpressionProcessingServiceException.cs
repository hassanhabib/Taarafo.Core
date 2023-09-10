// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Processings.PostImpressions.Exceptions
{
    public class FailedPostImpressionProcessingServiceException : Xeption
    {
        public FailedPostImpressionProcessingServiceException(Exception innerException)
            : base(
                message: "Failed Post Impression service occurred, please contact support",
                innerException: innerException)
        { }

        public FailedPostImpressionProcessingServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}