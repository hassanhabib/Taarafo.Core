// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostImpressions.Exceptions
{
	public class PostImpressionServiceException : Xeption
	{
		public PostImpressionServiceException(Exception innerException)
			: base(message: "Post impression service error occurred, please contact support.", innerException)
		{ }
	}
}
