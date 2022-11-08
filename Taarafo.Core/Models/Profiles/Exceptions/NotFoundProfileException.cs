// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Profiles.Exceptions
{
	public class NotFoundProfileException : Xeption
	{
		public NotFoundProfileException(Guid profileId)
				: base(message: $"Couldn't find profile with id: {profileId}.")
		{ }
	}
}
