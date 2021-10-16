// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;

namespace Taarafo.Core.Services.Foundations.Posts
{
    public partial class PostService
    {
        private void ValidatePost(Post post)
        {
            ValidatePostIsNotNull(post);

            Validate(
                (Rule: IsInvalid(post.Id), Parameter: nameof(Post.Id)),
                (Rule: IsInvalid(post.Content), Parameter: nameof(Post.Content)),
                (Rule: IsInvalid(post.Author), Parameter: nameof(Post.Author)),
                (Rule: IsInvalid(post.CreatedDate), Parameter: nameof(Post.CreatedDate)),
                (Rule: IsInvalid(post.UpdatedDate), Parameter: nameof(Post.UpdatedDate)),

                (Rule: IsNotSame(
                    firstDate: post.UpdatedDate,
                    secondDate: post.CreatedDate,
                    secondDateName: nameof(Post.CreatedDate)),
                Parameter: nameof(Post.UpdatedDate)),
                
                (Rule: IsNotRecent(post.CreatedDate), Parameter: nameof(Post.CreatedDate)));
        }

        private static void ValidatePostIsNotNull(Post post)
        {
            if (post is null)
            {
                throw new NullPostException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
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

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidPostException = new InvalidPostException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPostException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPostException.ThrowIfContainsErrors();
        }
    }
}
