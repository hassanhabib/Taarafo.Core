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
            : base(message: "Invalid post impression, Please correct the errors and try again.") 
        { }
    }
}
