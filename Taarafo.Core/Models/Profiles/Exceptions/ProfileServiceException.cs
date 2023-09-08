// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Profiles.Exceptions
{
	public class ProfileServiceException : Xeption
	{
		public ProfileServiceException(Exception innerException)
			: base(
				message: "Profile service error occurred, contact support.",
				innerException: innerException)
		{ }
		
		public ProfileServiceException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}