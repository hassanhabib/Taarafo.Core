// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.PostReports.Exceptions
{
    public class PostReportDependencyException : Xeption
    {
        public PostReportDependencyException(Xeption innerException)
            : base(message: "PostReport dependency error occurred, contact support.", innerException)
        { }
    }
}
