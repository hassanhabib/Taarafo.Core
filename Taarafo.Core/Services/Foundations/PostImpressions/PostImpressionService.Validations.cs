// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Profiles;

namespace Taarafo.Core.Services.Foundations.PostImpressions
{
    public partial class PostImpressionService
    {
        private void ValidatePostImpressionOnAdd(PostImpression postImpression)
        {
            ValidatePostImpressionIsNotNull(postImpression);

            Validate(
                (Rule: IsInvalid(postImpression.PostId), Parameter: nameof(PostImpression.PostId)),
                (Rule: IsInvalid(postImpression.Post), Parameter: nameof(PostImpression.Post)),
                (Rule: IsInvalid(postImpression.ProfileId), Parameter: nameof(PostImpression.ProfileId)),
                (Rule: IsInvalid(postImpression.Profile), Parameter: nameof(PostImpression.Profile)),
                (Rule: IsInvalid(postImpression.CreatedDate), Parameter: nameof(PostImpression.CreatedDate)),
                (Rule: IsInvalid(postImpression.UpdatedDate), Parameter: nameof(PostImpression.UpdatedDate)),

                (Rule: IsNotSame(
                    firstDate: postImpression.UpdatedDate,
                    secondDate: postImpression.CreatedDate,
                    secondDateName: nameof(PostImpression.CreatedDate)),
                Parameter: nameof(PostImpression.UpdatedDate)),

                (Rule: IsNotRecent(postImpression.CreatedDate), Parameter: nameof(PostImpression.CreatedDate)));
        }

        private static void ValidatePostImpressionIsNotNull(PostImpression postImpression)
        {
            if (postImpression is null)
            {
                throw new NullPostImpressionException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required."
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required."
        };

        private static dynamic IsInvalid(Post post) => new
        {
            Condition = post == null,
            Message = "Post is required."
        };

        private static dynamic IsInvalid(Profile profile) => new
        {
            Condition = profile == null,
            Message = "Profile is required."
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}."
            };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent."
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
            var invalidPostImpressionException = new InvalidPostImpressionException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPostImpressionException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPostImpressionException.ThrowIfContainsErrors();
        }
    }
}