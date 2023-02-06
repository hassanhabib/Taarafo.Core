// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostReports.Exceptions
{
    public class FailedPostPeportStorageException : Xeption
    {
        public FailedPostPeportStorageException(Exception innerException)
            : base(message: "Failed post report storage error occurred, contact support", innerException)
        { }
    }
}
