// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Events.Exceptions
{
    public class FailedEventStorageException : Xeption
    {
        public FailedEventStorageException(Exception innerException)
            : base (message: "Failed event storage error occurred, contact support.", innerException)
        { }
    }
}
