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
        private void ValidateCommentOnAdd(Comment comment)
        {
            ValidateCommentIsNotNull(comment);

            Validate(
                (Rule: IsInvalid(comment.Id), Parameter: nameof(Comment.Id)),
                (Rule: IsInvalid(comment.Content), Parameter: nameof(Comment.Content)),
                (Rule: IsInvalid(comment.CreatedDate), Parameter: nameof(Comment.CreatedDate)),
                (Rule: IsInvalid(comment.UpdatedDate), Parameter: nameof(Comment.UpdatedDate)),
                (Rule: IsInvalid(comment.PostId), Parameter: nameof(Comment.PostId)),

                (Rule: IsNotSame(
                    firstDate: comment.UpdatedDate,
                    secondDate: comment.CreatedDate,
                    secondDateName: nameof(Comment.CreatedDate)),
                Parameter: nameof(Comment.UpdatedDate)),

                (Rule: IsNotRecent(comment.CreatedDate), Parameter: nameof(Comment.CreatedDate)));
        }

        private void ValidateCommentOnModify(Comment comment)
        {
            ValidateCommentIsNotNull(comment);

            Validate(
                (Rule: IsInvalid(comment.Id), Parameter: nameof(Comment.Id)),
                (Rule: IsInvalid(comment.Content), Parameter: nameof(Comment.Content)),
                (Rule: IsInvalid(comment.CreatedDate), Parameter: nameof(Comment.CreatedDate)),
                (Rule: IsInvalid(comment.UpdatedDate), Parameter: nameof(Comment.UpdatedDate)),
                (Rule: IsInvalid(comment.PostId), Parameter: nameof(Comment.PostId)),

                (Rule: IsSame(
                    firstDate: comment.UpdatedDate,
                    secondDate: comment.CreatedDate,
                    secondDateName: nameof(Comment.CreatedDate)),
                Parameter: nameof(Comment.UpdatedDate)),

                (Rule: IsNotRecent(comment.UpdatedDate), Parameter: nameof(comment.UpdatedDate)));
        }

        public void ValidateCommentId(Guid commentId) =>
            Validate((Rule: IsInvalid(commentId), Parameter: nameof(Comment.Id)));

        private static void ValidateStorageComment(Comment maybeComment, Guid commentId)
        {
            if (maybeComment is null)
            {
                throw new NotFoundCommentException(commentId);
            }
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

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static void ValidateAginstStorageCommentOnModify(Comment inputComment, Comment storageComment)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputComment.CreatedDate,
                    secondDate: storageComment.CreatedDate,
                    secondDateName: nameof(Comment.CreatedDate)),
                Parameter: nameof(Comment.CreatedDate)),

                (Rule: IsNotSame(
                    firstDate: inputComment.UpdatedDate,
                    secondDate: storageComment.UpdatedDate,
                    secondDateName: nameof(Comment.UpdatedDate)),
                Parameter: nameof(Comment.UpdatedDate)));
        }

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
