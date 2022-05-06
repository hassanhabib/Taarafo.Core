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
                (Rule: IsInvalid(group.UpdatedDate), Parameter: nameof(Group.UpdatedDate)));
        }

        private static void ValidateGroupIsNotNull(Group group)
        {
            if (group is null)
            {
                throw new NullGroupException();
            }
        }

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