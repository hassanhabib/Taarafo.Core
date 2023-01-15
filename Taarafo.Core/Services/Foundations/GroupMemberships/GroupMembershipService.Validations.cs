// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Taarafo.Core.Models.GroupMemberships;
using Taarafo.Core.Models.GroupMemberships.Exceptions;

namespace Taarafo.Core.Services.Foundations.GroupMemberships
{
    public partial class GroupMembershipService
    {
        public void ValidateGroupMembershipOnAdd(GroupMembership groupMembership)
        {
            ValidateGroupMembershipIsNotNull(groupMembership);
        }

        private void ValidateGroupMembershipIsNotNull(GroupMembership groupMembership)
        {
            if (groupMembership is null)
            {
                throw new NullGroupMembershipException();
            }
        }



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

        }
    }
}