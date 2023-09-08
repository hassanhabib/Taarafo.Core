// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Profiles.Exceptions
{
	public class FailedProfileServiceException : Xeption
	{
		public FailedProfileServiceException(Exception innerException)
			: base(
				message: "Failed profile service occurred, please contact support",
				innerException: innerException)
		{ }
		
		public FailedProfileServiceException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}