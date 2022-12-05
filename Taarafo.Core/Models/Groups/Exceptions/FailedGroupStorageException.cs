// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Groups.Exceptions
{
	public class FailedGroupStorageException : Xeption
	{
		public FailedGroupStorageException(Exception innerException)
			: base(message: "Failed group storage error occurred, contact support.", innerException)
		{ }
	}
}
