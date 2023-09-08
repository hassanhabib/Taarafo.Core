// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Profiles.Exceptions
{
	public class InvalidProfileException : Xeption
	{
		public InvalidProfileException()
			: base(
				message: "Invalid profile. Please correct the errors and try again.")
		{ }
		
		public InvalidProfileException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}