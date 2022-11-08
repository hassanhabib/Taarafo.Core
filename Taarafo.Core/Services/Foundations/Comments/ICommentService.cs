// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Comments;

namespace Taarafo.Core.Services.Foundations.Comments
{
	public interface ICommentService
	{
		ValueTask<Comment> AddCommentAsync(Comment comment);
		ValueTask<Comment> RetrieveCommentByIdAsync(Guid commentId);
		IQueryable<Comment> RetrieveAllComments();
		ValueTask<Comment> ModifyCommentAsync(Comment comment);
		ValueTask<Comment> RemoveCommentByIdAsync(Guid commentId);
	}
}
