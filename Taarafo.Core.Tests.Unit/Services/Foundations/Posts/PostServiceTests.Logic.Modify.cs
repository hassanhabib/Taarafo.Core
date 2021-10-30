using System;
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
        public async Task ShouldModifyPostAsync()
        {
            // given
            DateTimeOffset randomDate = GetRadnomDateTimeOffset();
            DateTimeOffset randomInputDate = GetRadnomDateTimeOffset();
            Post randomPost = CreateRandomPost(randomInputDate);
            Post inputPost = randomPost;
            Post updatedStoragePost = inputPost.DeepClone();
            Post expectedPost = updatedStoragePost;
            Post storagePost = randomPost.DeepClone();
            inputPost.UpdatedDate = randomDate;
            Guid postId = inputPost.Id;

            this.dateTimeBrokerMock.Setup(broker =>
              broker.GetCurrentDateTimeOffset())
                  .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(postId))
                    .ReturnsAsync(storagePost);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdatePostAsync(inputPost))
                    .ReturnsAsync(updatedStoragePost);

            // when
            Post actualPost = await this.postService.ModifyPostAsync(inputPost);

            // then
            actualPost.Should().BeEquivalentTo(expectedPost);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(postId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePostAsync(inputPost),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
