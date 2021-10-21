using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task ShouldDeletePostAndLogItAsync()
        {
            // give
            Guid inputPostId = Guid.NewGuid();
            Post somePost = CreateRandomPost();
            Post inputPost = somePost;
            Post storagePost = inputPost;
            Post expectedPost = storagePost.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(inputPostId))
                    .ReturnsAsync(storagePost);

            this.storageBrokerMock.Setup(broker =>
                broker.DeletePostAsync(inputPost))
                    .ReturnsAsync(storagePost);

            // when
            Post actualPost =
                await this.postService.RemovePostByIdAsync(inputPostId);

            // then
            actualPost.Should().BeEquivalentTo(expectedPost);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(inputPostId), 
                    Times.Once);

            this.storageBrokerMock.Verify(broker => 
                broker.DeletePostAsync(inputPost),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();               
        }
    }
}
