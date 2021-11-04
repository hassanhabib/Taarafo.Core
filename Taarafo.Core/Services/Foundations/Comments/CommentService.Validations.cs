// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Taarafo.Core.Models.Comments;
using Taarafo.Core.Models.Comments.Exceptions;

namespace Taarafo.Core.Services.Foundations.Comments
{
    public partial class CommentService
    {
        private void ValidateComment(Comment post)
        {
            ValidateCommentIsNotNull(post);
        }

        private static void ValidateCommentIsNotNull(Comment post)
        {
            if (post is null)
            {
                throw new NullCommentException();
            }
        }
    }
}
