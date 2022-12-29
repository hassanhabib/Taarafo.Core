// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Taarafo.Core.Models.GroupPosts;
using Taarafo.Core.Models.GroupPosts.Exceptions;

namespace Taarafo.Core.Services.Foundations.GroupPosts
{
    public partial class GroupPostService
    {
        public void ValidateGroupPostId(Guid groupPostId)
        {
            Validate(
                (Rule: IsInvalid(groupPostId), Parameter: nameof(GroupPost.GroupId)),
                (Rule: IsInvalid(groupPostId), Parameter: nameof(GroupPost.PostId)));
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidGroupPostException = new InvalidGroupPostException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidGroupPostException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidGroupPostException.ThrowIfContainsErrors();
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };
    }
}