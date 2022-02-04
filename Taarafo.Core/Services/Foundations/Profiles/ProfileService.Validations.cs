// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Profiles;
using Taarafo.Core.Models.Profiles.Exceptions;

namespace Taarafo.Core.Services.Foundations.Profiles
{
    public partial class ProfileService
    {
        private void ValidateProfileOnAdd(Profile profile)
        {
            ValidateProfileIsNotNull(profile);
            Validate(
                (Rule: IsInvalid(profile.Id), Parameter: nameof(Profile.Id)),
                (Rule: IsInvalid(profile.Name), Parameter: nameof(Profile.Name)),
                (Rule: IsInvalid(profile.Username), Parameter: nameof(Profile.Username)),
                (Rule: IsInvalid(profile.Email), Parameter: nameof(Profile.Email)),
                (Rule: IsInvalid(profile.CreatedDate), Parameter: nameof(Profile.CreatedDate)),
                (Rule: IsInvalid(profile.UpdatedDate), Parameter: nameof(Profile.UpdatedDate)));
        }

        private void ValidateProfileIsNotNull(Profile profile)
        {
            if (profile is null)
            {
                throw new NullProfileException();
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

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidProfileException =
                new InvalidProfileException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidProfileException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidProfileException.ThrowIfContainsErrors();
        }
    }
}
