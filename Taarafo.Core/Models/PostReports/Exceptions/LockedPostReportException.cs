// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostReports.Exceptions
{
    public class LockedPostReportException : Xeption
    {
        public LockedPostReportException(Exception innerException)
            : base(
                message: "PostReport is locked, please try again.",
                innerException: innerException)
        { }
        
        public LockedPostReportException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}