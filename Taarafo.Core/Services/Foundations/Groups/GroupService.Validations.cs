// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.Groups;
using Taarafo.Core.Models.Groups.Exceptions;

namespace Taarafo.Core.Services.Foundations.Groups
{
    public partial class GroupService
    {
        private void ValidateGroupOnAdd(Group group)
        {
            ValidateGroupIsNotNull(group);

            Validate(
                (Rule: IsInvalid(group.Id), Parameter: nameof(Group.Id)),
                (Rule: IsInvalid(group.Name), Parameter: nameof(Group.Name)),
                (Rule: IsInvalid(group.Description), Parameter: nameof(Group.Description)),
                (Rule: IsInvalid(group.CreatedDate), Parameter: nameof(Group.CreatedDate)),
                (Rule: IsInvalid(group.UpdatedDate), Parameter: nameof(Group.UpdatedDate)),

                (Rule: IsNotSame(
                    firstDate: group.UpdatedDate,
                    secondDate: group.CreatedDate,
                    secondDateName: nameof(Group.CreatedDate)),
                Parameter: nameof(Group.UpdatedDate)),

                (Rule: IsNotRecent(group.CreatedDate), Parameter: nameof(Group.CreatedDate)));
        }

        private void ValidateGroupOnModify(Group group)
        {
            ValidateGroupIsNotNull(group);

            Validate(
                (Rule: IsInvalid(group.Id), Parameter: nameof(Group.Id)),
                (Rule: IsInvalid(group.Name), Parameter: nameof(Group.Name)),
                (Rule: IsInvalid(group.Description), Parameter: nameof(Group.Description)),
                (Rule: IsInvalid(group.CreatedDate), Parameter: nameof(Group.CreatedDate)),
                (Rule: IsInvalid(group.UpdatedDate), Parameter: nameof(Group.UpdatedDate)),
                
                (Rule: IsSame(
                    firstDate: group.UpdatedDate,
                    secondDate: group.CreatedDate,
                    secondDateName: nameof(Group.CreatedDate)),
                Parameter: nameof(Group.UpdatedDate)),
                
                (Rule: IsNotRecent(group.UpdatedDate), Parameter: nameof(Group.UpdatedDate)));
        }

        private static void ValidateGroupIsNotNull(Group group)
        {
            if (group is null)
            {
                throw new NullGroupException();
            }
        }

        private void ValidateStorageGroup(Group maybeGroup, Guid groupId)
        {
            if (maybeGroup is null)
            {
                throw new NotFoundGroupException(groupId);
            }
        }

        private void ValidateGroupId(Guid groupId) =>
            throw new NotImplementedException();

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
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

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidGroupException =
                new InvalidGroupException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidGroupException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidGroupException.ThrowIfContainsErrors();
        }
    }
}