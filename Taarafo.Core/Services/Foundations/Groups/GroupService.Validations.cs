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
        public void ValidateGroupId(Guid groupId) =>
            Validate((Rule: IsInvalid(groupId), Parameter: nameof(Group.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void ValidateStorageGroup(Group maybeGroup, Guid groupId)
        {
            if (maybeGroup is null)
            {
                throw new NotFoundGroupException(groupId);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidGroupException = new InvalidGroupException();

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