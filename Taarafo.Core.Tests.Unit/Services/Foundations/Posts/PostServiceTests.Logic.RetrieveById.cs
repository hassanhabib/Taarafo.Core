// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.Posts;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Posts
{
    public partial class PostServiceTests
    {
        [Fact]
        public async Task ShouldRetrievePostByIdAsync()
        {
            // given
            Post randomPosts = CreateRandomPost();
            Guid inputPostId = Guid.NewGuid();
            Post storagePosts = randomPosts;
            Post expectedPosts = storagePosts.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(inputPostId))
                    .ReturnsAsync(storagePosts);

            // when
            Post actualPosts =
                await this.postService.RetrievePostByIdAsync(inputPostId);

            // then
            actualPosts.Should().BeEquivalentTo(expectedPosts);
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPosts(),
                    Times.Once);
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
