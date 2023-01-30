// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostReports.Exceptions
{
    public class AlreadyExistsPostReportException : Xeption
    {
        public AlreadyExistsPostReportException(Exception innerException)
            : base(message: "PostReport already exists.", innerException)
        { }
    }
}
