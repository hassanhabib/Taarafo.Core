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
        public static void ValidateGroupPostOnAdd(GroupPost groupPost)
        {
            ValidateGroupPostIsNotNull(groupPost);

            Validate(
                (Rule: IsInvalid(groupPost.GroupId), Parameter: nameof(GroupPost.GroupId)),
                (Rule: IsInvalid(groupPost.PostId), Parameter: nameof(GroupPost.PostId)));
        }

        private void ValidateGroupPostId(Guid groupId, Guid postId) =>
            Validate(
                (Rule: IsInvalid(groupId), Parameter: nameof(GroupPost.GroupId)),
                (Rule: IsInvalid(postId), Parameter: nameof(GroupPost.PostId)));

        private static void ValidateStorageGroupPostExists(GroupPost maybeGroupPost, Guid groupId, Guid postId)
        {
            if (maybeGroupPost is null)
            {
                throw new NotFoundGroupPostException(groupId, postId);
            }
        }

        private void ValidateGroupPostOnModify(GroupPost groupPost)
        {
            ValidateGroupPostIsNotNull(groupPost);

            Validate(
                (Rule: IsInvalid(groupPost.GroupId), Parameter: nameof(GroupPost.GroupId)),
                (Rule: IsInvalid(groupPost.PostId), Parameter: nameof(GroupPost.PostId)),
                (Rule: IsInvalid(groupPost.CreatedDate), Parameter: nameof(GroupPost.CreatedDate)),
                (Rule: IsInvalid(groupPost.UpdatedDate), Parameter: nameof(GroupPost.UpdatedDate)),
                (Rule: IsNotRecent(groupPost.UpdatedDate), Parameter: nameof(GroupPost.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: groupPost.UpdatedDate,
                    secondDate: groupPost.CreatedDate,
                    secondDateName: nameof(GroupPost.CreatedDate)),

                Parameter: nameof(GroupPost.UpdatedDate)));
        }

        private static void ValidateAginstStorageGroupPostOnModify(GroupPost inputGroupPost, GroupPost storageGroupPost)
        {
            ValidateStorageGroupPostExists(storageGroupPost, inputGroupPost.GroupId, inputGroupPost.PostId);
        }

        private void ValidateStorageGroupPost(GroupPost maybeGroupPost, Guid groupId, Guid postId)
        {
            if (maybeGroupPost is null)
            {
                throw new NotFoundGroupPostException(groupId, postId);
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(object @object) => new
        {
            Condition = @object is null,
            Message = "Object is required"
        };
        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Value is required"
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
            DateTimeOffset currentDateTime = this.dateTimeBrokerMock.GetCurrentDateTimeOffset();
            TimeSpan timeDifference = currentDateTime.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static void ValidateGroupPostIsNotNull(GroupPost groupPost)
        {
            if (groupPost is null)
            {
                throw new NullGroupPostException();
            }
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
    }
}
