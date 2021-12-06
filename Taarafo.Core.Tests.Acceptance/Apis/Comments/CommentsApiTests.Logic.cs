// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
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
    }
}
