// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.Comments;
using Taarafo.Core.Models.Comments.Exceptions;

namespace Taarafo.Core.Services.Foundations.Comments
{
    public partial class CommentService
    {
        private void ValidateComment(Comment comment)
        {
            ValidateCommentIsNotNull(comment);

            Validate(
                (Rule: IsInvalid(comment.Id), Parameter: nameof(Comment.Id)),
                (Rule: IsInvalid(comment.Content), Parameter: nameof(Comment.Content)),
                (Rule: IsInvalid(comment.CreatedDate), Parameter: nameof(Comment.CreatedDate)),
                (Rule: IsInvalid(comment.UpdatedDate), Parameter: nameof(Comment.UpdatedDate)),
                (Rule: IsInvalid(comment.PostId), Parameter: nameof(Comment.PostId)));
        }

        private static void ValidateCommentIsNotNull(Comment comment)
        {
            if (comment is null)
            {
                throw new NullCommentException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidCommentException = new InvalidCommentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidCommentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidCommentException.ThrowIfContainsErrors();
        }
    }
}
