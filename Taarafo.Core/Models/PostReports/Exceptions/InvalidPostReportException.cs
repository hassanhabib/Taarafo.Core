// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostReports.Exceptions
{
    public class InvalidPostReportException : Xeption
    {
        public InvalidPostReportException()
            : base(message: "PostReport is invalid.")
        { }
    }
}