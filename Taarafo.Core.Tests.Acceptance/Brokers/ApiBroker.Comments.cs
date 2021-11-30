// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taarafo.Core.Tests.Acceptance.Models.Comments;

namespace Taarafo.Core.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string CommentsRelativeUrl = "api/comments";

        public async ValueTask<Comment> CommentCommentAsync(Comment comment) =>
            await this.apiFactoryClient.PostContentAsync(CommentsRelativeUrl, comment);

        public async ValueTask<List<Comment>> GetAllCommentsAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<Comment>>($"{CommentsRelativeUrl}/");

        public async ValueTask<Comment> GetCommentByIdAsync(Guid commentId) =>
            await this.apiFactoryClient.GetContentAsync<Comment>($"{CommentsRelativeUrl}/{commentId}");

        public async ValueTask<Comment> DeleteCommentByIdAsync(Guid commentId) =>
            await this.apiFactoryClient.DeleteContentAsync<Comment>($"{CommentsRelativeUrl}/{commentId}");
    }
}
