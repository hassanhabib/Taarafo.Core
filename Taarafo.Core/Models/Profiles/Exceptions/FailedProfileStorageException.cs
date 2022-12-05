// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Profiles.Exceptions
{
	public class FailedProfileStorageException : Xeption
	{
		public FailedProfileStorageException(Exception innerException)
			: base(message: "Failed profile storage error occurred, contact support.", innerException)
		{ }
	}
}
