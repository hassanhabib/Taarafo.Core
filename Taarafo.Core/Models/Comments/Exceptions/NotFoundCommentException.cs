// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Comments.Exceptions
{
	public class NotFoundCommentException : Xeption
	{
		public NotFoundCommentException(Guid commentId)
			: base(message: $"Couldn't find comment with id: {commentId}.")
		{ }
	}
}
