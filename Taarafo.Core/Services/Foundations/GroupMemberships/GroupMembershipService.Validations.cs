// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.GroupMemberships;
using Taarafo.Core.Models.GroupMemberships.Exceptions;

namespace Taarafo.Core.Services.Foundations.GroupMemberships
{
    public partial class GroupMembershipService
    {
        public void ValidateGroupMembershipOnAdd(GroupMembership groupMembership)
        {
            ValidateGroupMembershipIsNotNull(groupMembership);

            Validate(
                (Rule: IsInvalid(groupMembership.Id), Parameter: nameof(GroupMembership.Id)),
                (Rule: IsInvalid(groupMembership.GroupId), Parameter: nameof(GroupMembership.GroupId)),
                (Rule: IsInvalid(groupMembership.ProfileId), Parameter: nameof(GroupMembership.ProfileId)),
                (Rule: IsInvalid(groupMembership.MembershipDate), Parameter: nameof(GroupMembership.MembershipDate)));
        }

        private void ValidateGroupMembershipIsNotNull(GroupMembership groupMembership)
        {
            if (groupMembership is null)
            {
                throw new NullGroupMembershipException();
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


        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidGroupMembershipException =
                new InvalidGroupMembershipException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidGroupMembershipException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }
            invalidGroupMembershipException.ThrowIfContainsErrors();
        }
    }
}