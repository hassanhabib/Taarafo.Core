// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;

namespace Taarafo.Core.Services.Foundations.Posts
{
    public partial class PostService
    {
        private static void ValidatePost(Post post)
        {
            ValidatePostIsNotNull(post);

            Validate(
                (Rule: IsInvalid(post.Id), Parameter: nameof(Post.Id)),
                (Rule: IsInvalid(post.Content), Parameter: nameof(Post.Content)),
                (Rule: IsInvalid(post.Author), Parameter: nameof(Post.Author)),
                (Rule: IsInvalid(post.CreatedDate), Parameter: nameof(Post.CreatedDate)),
                (Rule: IsInvalid(post.UpdatedDate), Parameter: nameof(Post.UpdatedDate)));
        }

        private static void ValidatePostIsNotNull(Post post)
        {
            if (post is null)
            {
                throw new NullPostException();
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
            var invalidPostException = new InvalidPostException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPostException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPostException.ThrowIfContainsErrors();
        }
    }
}
