// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
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
                (Rule: IsInvalid(profile.UpdatedDate), Parameter: nameof(Profile.UpdatedDate)),

                (Rule: IsNotSame(
                    firstDate: profile.UpdatedDate,
                    secondDate: profile.CreatedDate,
                    secondDateName: nameof(Profile.CreatedDate)),
                Parameter: nameof(Profile.UpdatedDate)),

                (Rule: IsNotRecent(profile.CreatedDate), Parameter: nameof(Profile.CreatedDate)));
        }

        private void ValidateProfileIsNotNull(Profile profile)
        {
            if (profile is null)
            {
                throw new NullProfileException();
            }
        }

        private void ValidateStorageProfile(Profile maybeProfile, Guid profileId)
        {
            if (maybeProfile is null)
            {
                throw new NotFoundProfileException(profileId);
            }
        }

        private void ValidateProfileId(Guid profileId) =>
            Validate((Rule: IsInvalid(profileId), Parameter: nameof(Profile.Id)));

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
