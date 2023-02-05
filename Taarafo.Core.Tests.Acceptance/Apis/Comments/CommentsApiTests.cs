// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taarafo.Core.Tests.Acceptance.Brokers;
using Taarafo.Core.Tests.Acceptance.Models.Comments;
using Taarafo.Core.Tests.Acceptance.Models.Posts;
using Tynamix.ObjectFiller;
using Xunit;

namespace Taarafo.Core.Tests.Acceptance.Apis.Comments
{
	[Collection(nameof(ApiTestCollection))]
	public partial class CommentsApiTests
	{
		private readonly ApiBroker apiBroker;

		public CommentsApiTests(ApiBroker apiBroker) =>
			this.apiBroker = apiBroker;

		private async ValueTask<List<Comment>> CreateRandomCommentsAsync()
		{
			int randomNumber = GetRandomNumber();
			var randomComments = new List<Comment>();

			for (int i = 0; i < randomNumber; i++)
			{
				randomComments.Add(await PostRandomCommentAsync());
			}
			return randomComments;
		}

		private int GetRandomNumber() =>
			new IntRange(min: 2, max: 10).GetValue();

		private async ValueTask<Comment> PostRandomCommentAsync()
		{
			Comment randomComment = await CreateRandomComment();
			await this.apiBroker.PostCommentAsync(randomComment);

			return randomComment;
		}

		private async ValueTask<Comment> CreateRandomComment()
		{
			Post post = await PostRandomPostAsync();
			DateTimeOffset now = DateTimeOffset.UtcNow;
			var filler = new Filler<Comment>();

			filler.Setup()
				.OnProperty(comment => comment.CreatedDate).Use(now)
				.OnProperty(comment => comment.UpdatedDate).Use(now)
				.OnProperty(comment => comment.PostId).Use(post.Id);

			return filler.Create();
		}

		private async ValueTask<Post> PostRandomPostAsync()
		{
			Post randomPost = CreateRandomPost();
			await this.apiBroker.PostPostAsync(randomPost);

			return randomPost;
		}

		private static DateTimeOffset GetRandomDateTime() =>
			new DateTimeRange(earliestDate: new DateTime()).GetValue();

		private static Comment UpdateCommentWithRandomValues(Comment inputComment)
		{
			DateTimeOffset now = DateTimeOffset.UtcNow;

			var filler = new Filler<Comment>();

			filler.Setup()
				.OnProperty(comment => comment.Id).Use(inputComment.Id)
				.OnProperty(comment => comment.PostId).Use(inputComment.PostId)
				.OnProperty(comment => comment.CreatedDate).Use(inputComment.CreatedDate)
				.OnProperty(comment => comment.UpdatedDate).Use(now)
				.OnType<DateTimeOffset>().Use(GetRandomDateTime());

			return filler.Create();
		}

		private static Post CreateRandomPost() =>
			CreateRandomPostFiller().Create();

		private static Filler<Post> CreateRandomPostFiller()
		{
			DateTimeOffset now = DateTimeOffset.UtcNow;
			var filler = new Filler<Post>();

			filler.Setup()
				.OnProperty(post => post.CreatedDate).Use(now)
				.OnProperty(post => post.UpdatedDate).Use(now);

			return filler;
		}

		private async ValueTask<Comment> DeleteCommentAsync(Comment actualComment)
		{
			Comment deletedComment =
				await apiBroker.DeleteCommentByIdAsync(actualComment.Id);

			await apiBroker.DeletePostByIdAsync(actualComment.PostId);

			return deletedComment;
		}
	}
}
