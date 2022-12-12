// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.PostImpressions.Exceptions
{
	public class InvalidPostImpressionException : Xeption
	{
        public InvalidPostImpressionException(string parameterName, object parameterValue)
            : base(message: $"Invalid postImpression, " +
                 $"parameter name: {parameterName}, " +
                 $"parameter value: {parameterValue}.")
        { }

        public InvalidPostImpressionException()
			: base(message: "Invalid post impression. Please correct the errors and try again.")
		{ }
	}
}
