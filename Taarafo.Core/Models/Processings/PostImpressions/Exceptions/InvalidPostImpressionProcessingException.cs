// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Processings.PostImpressions.Exceptions
{
    public class InvalidPostImpressionProcessingException : Xeption
    {
        public InvalidPostImpressionProcessingException()
            : base(message: "Invalid Post Impression, Please correct the errors and try again.")
        { }

        public InvalidPostImpressionProcessingException(string message)
            : base(message)
        { }
    }
}