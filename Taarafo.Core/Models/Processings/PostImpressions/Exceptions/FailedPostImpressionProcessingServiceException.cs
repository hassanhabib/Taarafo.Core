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
            : base(message: "Failed post impression service occurred, please contact support", innerException)
        { }
    }
}
