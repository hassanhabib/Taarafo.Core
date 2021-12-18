// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using RESTFulSense.Exceptions;
using Taarafo.Core.Tests.Acceptance.Models.Comments;
using Xunit;

namespace Taarafo.Core.Tests.Acceptance.Apis.Comments
{
    public partial class CommentsApiTests
    {
        [Fact]
        public async Task ShouldPostCommentAsync()
        {
            // given
            Comment randomComment = await CreateRandomComment();
            Comment inputComment = randomComment;
            Comment expectedComment = inputComment;

            // when 
            await this.apiBroker.PostCommentAsync(inputComment);

            Comment actualComment =
                 await this.apiBroker.GetCommentByIdAsync(inputComment.Id);

            // then
            actualComment.Should().BeEquivalentTo(expectedComment);
            await DeleteCommentAsync(actualComment);
        }

        [Fact]
        public async Task ShouldGetAllCommentsAsync()
        {
            // given
            List<Comment> randomComments = await CreateRandomCommentsAsync();
            List<Comment> expectedComments = randomComments;

            // when
            List<Comment> actualComments = await this.apiBroker.GetAllCommentsAsync();

            // then
            foreach (Comment expectedComment in expectedComments)
            {
                Comment actualComment = actualComments.Single(comment => comment.Id == expectedComment.Id);
                actualComment.Should().BeEquivalentTo(expectedComment);
                await this.apiBroker.DeleteCommentByIdAsync(actualComment.Id);
            }
        }

        [Fact]
        public async Task ShouldGetCommentByIdAsync()
        {
            // given
            Comment randomComment = await PostRandomCommentAsync();
            Comment expectedComment = randomComment;

            // when
            Comment actualComment = await this.apiBroker.GetCommentByIdAsync(randomComment.Id);

            // then
            actualComment.Should().BeEquivalentTo(expectedComment);
            await this.apiBroker.DeleteCommentByIdAsync(actualComment.Id);
        }

        [Fact]
        public async Task ShouldPutCommentAsync()
        {
            // given
            Comment randomComment = await PostRandomCommentAsync();
            Comment modifiedComment = UpdateRandomComment(randomComment);

            // when
            await this.apiBroker.PutCommentAsync(modifiedComment);

            Comment actualComment = await this.apiBroker.GetCommentByIdAsync(randomComment.Id);

            // then
            actualComment.Should().BeEquivalentTo(modifiedComment);
            await this.apiBroker.DeleteCommentByIdAsync(actualComment.Id);
        }

        [Fact]
        public async Task ShouldDeleteCommentAsync()
        {
            // given
            Comment randomComment = await PostRandomCommentAsync();
            Comment inputComment = randomComment;
            Comment expectedComment = inputComment;

            // when
            Comment deletedComment =
                await this.apiBroker.DeleteCommentByIdAsync(inputComment.Id);

            ValueTask<Comment> getCommentbyIdTask =
                this.apiBroker.GetCommentByIdAsync(inputComment.Id);

            // then
            deletedComment.Should().BeEquivalentTo(expectedComment);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
                getCommentbyIdTask.AsTask());
        }
    }
}
