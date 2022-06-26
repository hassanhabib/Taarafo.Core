// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostImpressions.Exceptions
{
    public class FailedPostImpressionStorageException : Xeption
    {
        public FailedPostImpressionStorageException(Exception innerException)
            : base(message: "Failed post impression storage error has occurred, contact support.", innerException)
        { }
    }
}
