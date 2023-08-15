// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Comments.Exceptions
{
	public class CommentValidationException : Xeption
	{
		public CommentValidationException(Xeption innerException)
			: base(
				message: "Comment validation errors occurred, please try again.",
					innerException: innerException) { }

		public CommentValidationException(string message, Xeption innerException)
			: base(message, innerException) { }
	}
}