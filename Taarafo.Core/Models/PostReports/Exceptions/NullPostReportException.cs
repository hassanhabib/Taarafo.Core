// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.PostReports.Exceptions
{
    public class NullPostReportException : Xeption
    {
        public NullPostReportException()
            : base(message: "PostReport is null.")
        { }
    }
}
