// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Processings.PostImpressions.Exceptions
{
    public class PostImpressionProcessingServiceException : Xeption
    {
        public PostImpressionProcessingServiceException(Exception innerException)
            : base(
                message: "Failed Post Impression external service occurred, please contact support",
                innerException: innerException)
        { }

        public PostImpressionProcessingServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}