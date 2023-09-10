// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.PostReports.Exceptions
{
    public class PostReportDependencyValidationException : Xeption
    {
        public PostReportDependencyValidationException(Xeption innerException)
            : base(
                message: "PostReport dependency validation error occurred, fix the errors and try again.",
                innerException: innerException)
        { }
        
        public PostReportDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}