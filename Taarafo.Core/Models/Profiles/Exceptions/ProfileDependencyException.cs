// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Profiles.Exceptions
{
	public class ProfileDependencyException : Xeption
	{
		public ProfileDependencyException(Xeption innerException)
			: base(message: "Profile dependency error occurred, contact support.", innerException)
		{ }
	}
}
