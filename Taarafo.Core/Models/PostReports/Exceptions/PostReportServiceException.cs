// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostReports.Exceptions
{
    public class PostReportServiceException : Xeption
    {
        public PostReportServiceException(Exception innerException)
            : base(message: "Post report service error occurred, contact support.", innerException)
        { }
    }
}
