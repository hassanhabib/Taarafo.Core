// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.PostImpressions.Exceptions
{
    public class InvalidPostImpressionException : Xeption
    {
        public InvalidPostImpressionException()
            : base(
                message: "Invalid post impression. Please correct the errors and try again.")
        { }

        public InvalidPostImpressionException(string message)
            : base(message)
        { }
    }
}