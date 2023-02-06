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
            : base(message: "PostReport validation errors occurred, please try again.",
                innerException)
        { }
    }
}
