// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.PostReports.Exceptions
{
    public class PostReportValidationException : Xeption
    {
        public PostReportValidationException(Xeption innerException)
            : base(
                message: "PostReport validation errors occurred, please try again.",
                innerException: innerException)
        { }
        
        public PostReportValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}