// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Groups.Exceptions
{
	public class GroupDependencyException : Xeption
	{
		public GroupDependencyException(Xeption innerException)
			: base(message: "Group dependency error occurred, contact support.", innerException)
		{ }
	}
}
