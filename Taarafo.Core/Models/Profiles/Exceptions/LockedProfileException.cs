// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Profiles.Exceptions
{
	public class LockedProfileException : Xeption
	{
		public LockedProfileException(Exception innerException)
			: base(
				message: "Locked profile record exception, please try again later",
				innerException: innerException)
		{ }
		
		public LockedProfileException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}