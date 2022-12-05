// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Groups.Exceptions
{
	public class LockedGroupException : Xeption
	{
		public LockedGroupException(Exception innerException)
			: base(message: "Locked group record exception, please try again later", innerException)
		{ }
	}
}