// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.Processings.PostImpressions.Exceptions;

namespace Taarafo.Core.Services.Processings.PostImpressions
{
    public partial class PostImpressionProcessingService
    {
        private static void ValidatePostImpression(PostImpression postImpression)
        {
            ValidatePostImpressionIsNotNull(postImpression);

            Validate(
                (Rule: IsInvalid(postImpression.PostId), Parameter: nameof(PostImpression.PostId)),
                (Rule: IsInvalid(postImpression.ProfileId), Parameter: nameof(PostImpression.ProfileId)));
        }

        private static void ValidatePostImpressionIsNotNull(PostImpression postImpression)
        {
            if (postImpression is null)
            {
                throw new NullPostImpressionProcessingException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidPostImpressionProcessingException =
                new InvalidPostImpressionProcessingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPostImpressionProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPostImpressionProcessingException.ThrowIfContainsErrors();
        }
    }
}