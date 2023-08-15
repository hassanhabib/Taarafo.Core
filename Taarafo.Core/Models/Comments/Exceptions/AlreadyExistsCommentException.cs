// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Comments.Exceptions
{
	public class AlreadyExistsCommentException : Xeption
	{
		public AlreadyExistsCommentException(Exception innerException)
			: base(
				message: "Comment with the same id already exists, please correct.",
					innerException: innerException) { }

		public AlreadyExistsCommentException(string message, Exception innerException)
			: base(message, innerException) { }
	}
}