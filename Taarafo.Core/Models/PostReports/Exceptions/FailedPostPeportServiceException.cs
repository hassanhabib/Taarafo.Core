// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostReports.Exceptions
{
    public class FailedPostPeportServiceException : Xeption
    {
        public FailedPostPeportServiceException(Exception innerException)
            : base(message: "Failed post report service occurred, please contact support.", innerException)
        { }
    }
}
