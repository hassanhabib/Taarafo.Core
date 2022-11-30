// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Posts.Exceptions
{
	public class LockedPostException : Xeption
	{
		public LockedPostException(Exception innerException)
			: base(message: "Locked post record exception, please try again later", innerException) { }
	}
}
