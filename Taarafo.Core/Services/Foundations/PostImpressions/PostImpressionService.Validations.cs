// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;

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
                (Rule: IsInvalid(postImpression.Impression), Parameter: nameof(PostImpression.Impression)),

                (Rule: IsNotSame(
                    firstDate: postImpression.UpdatedDate,
                    secondDate: postImpression.CreatedDate,
                    secondDateName: nameof(PostImpression.CreatedDate)),
                Parameter: nameof(PostImpression.UpdatedDate)),

                (Rule: IsNotRecent(postImpression.CreatedDate), Parameter: nameof(PostImpression.CreatedDate)));
        }

        private void ValidatePostImpressionOnModify(PostImpression postImpression)
        {
            ValidatePostImpressionIsNotNull(postImpression);

            Validate(
                (Rule: IsInvalid(postImpression.PostId), Parameter: nameof(PostImpression.PostId)),
                (Rule: IsInvalid(postImpression.ProfileId), Parameter: nameof(PostImpression.ProfileId)),
                (Rule: IsInvalid(postImpression.CreatedDate), Parameter: nameof(PostImpression.CreatedDate)),
                (Rule: IsInvalid(postImpression.UpdatedDate), Parameter: nameof(PostImpression.UpdatedDate)),
                (Rule: IsNotRecent(postImpression.UpdatedDate), Parameter: nameof(PostImpression.UpdatedDate)),

                (Rule: IsSame(
                firstDate: postImpression.UpdatedDate,
                secondDate: postImpression.CreatedDate,
                secondDateName: nameof(postImpression.CreatedDate)),

                Parameter: nameof(PostImpression.UpdatedDate)));
        }

        private static void ValidateAginstStoragePostImpressionOnModify(
            PostImpression inputPostImpression, PostImpression storagePostImpression)
        {
            ValidateStoragePostImpression(
                storagePostImpression,
                inputPostImpression.PostId,
                inputPostImpression.ProfileId);

            Validate(
                (Rule: IsNotSame(
                    firstDate: inputPostImpression.CreatedDate,
                    secondDate: storagePostImpression.CreatedDate,
                    secondDateName: nameof(PostImpression.CreatedDate)),

                 Parameter: nameof(PostImpression.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputPostImpression.UpdatedDate,
                    secondDate: storagePostImpression.UpdatedDate,
                    secondDateName: nameof(PostImpression.UpdatedDate)),

                 Parameter: nameof(PostImpression.UpdatedDate)));
        }

        private void ValidatePostImpressionId(Guid postId, Guid profileId) =>
            Validate(
                (Rule: IsInvalid(postId), Parameter: nameof(PostImpression.PostId)),
                (Rule: IsInvalid(profileId), Parameter: nameof(PostImpression.ProfileId)));

        private static void ValidatePostImpressionIsNotNull(PostImpression postImpression)
        {
            if (postImpression is null)
            {
                throw new NullPostImpressionException();
            }
        }

        private static void ValidatePostImpressionOnRemove(PostImpression postImpression)
        {
            ValidatePostImpressionIsNotNull(postImpression);

            Validate(
                (Rule: IsInvalid(postImpression.PostId), Parameter: nameof(PostImpression.PostId)),
                (Rule: IsInvalid(postImpression.ProfileId), Parameter: nameof(PostImpression.ProfileId)));
        }

        private static void ValidateStoragePostImpression(
            PostImpression maybePostImpression, Guid postId, Guid profileId)
        {
            if (maybePostImpression is null)
            {
                throw new NotFoundPostImpressionException(postId, profileId);
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

        private static dynamic IsInvalid(object @object) => new
        {
            Condition = @object is null,
            Message = "Object is required"
        };

        private static dynamic IsInvalid(PostImpressionType type) => new
        {
            Condition = Enum.IsDefined(type) is false,
            Message = "Value is not recognized"
        };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
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