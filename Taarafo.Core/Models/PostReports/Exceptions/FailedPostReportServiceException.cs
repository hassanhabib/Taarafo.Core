// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostReports.Exceptions
{
    public class FailedPostReportServiceException : Xeption
    {
        public FailedPostReportServiceException(Exception innerException)
            : base(message: "Failed post report service occurred, please contact support.", innerException)
        { }
    }
}
