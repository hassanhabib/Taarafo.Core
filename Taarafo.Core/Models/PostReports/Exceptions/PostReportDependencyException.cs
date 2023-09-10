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
            : base(
                message: "Post report dependency validation occurred, please try again.",
                innerException: innerException)
        { }
        
        public PostReportDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}