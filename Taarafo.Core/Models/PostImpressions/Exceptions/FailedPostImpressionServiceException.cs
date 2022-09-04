// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostImpressions.Exceptions
{
    public class FailedPostImpressionServiceException : Xeption
    {
        public FailedPostImpressionServiceException(Exception innerException)
            : base(message: "Failed post impression service occurred, please contact support.", innerException)
        { }
    }
}
