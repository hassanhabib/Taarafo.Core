// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostReports.Exceptions
{
    public class InvalidPostReportReferenceException : Xeption
    {
        public InvalidPostReportReferenceException(Exception innerException)
            : base(message: "Invalid PostReport reference error occurred.",
                innerException)
        { }
    }
}


