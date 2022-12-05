// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Profiles.Exceptions
{
	public class ProfileValidationException : Xeption
	{
		public ProfileValidationException(Xeption innerException)
			: base(message: "Profile validation errors occurred, please try again.",
				  innerException)
		{ }
	}
}
