using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Posts
{
    public partial class PostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfPostIsNullAndLogItAsync()
        {
            // given
            Post nullPost = null;
            var nullPostException = new NullPostException();

            var expectedPostValidationException =
                new PostValidationException(nullPostException);

            // when
            ValueTask<Post> modifyPostTask =
                this.postService.ModifyPostAsync(nullPost);

            // then
            await Assert.ThrowsAsync<PostValidationException>(() =>
                modifyPostTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameValidationExceptionAs(expectedPostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePostAsync(It.IsAny<Post>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfPostIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidPost = new Post
            {
                Content = invalidText
            };

            var invalidPostException = new InvalidPostException();

            invalidPostException.AddData(
                key: nameof(Post.Id),
                values: "Id is required");

            invalidPostException.AddData(
                key: nameof(Post.Content),
                values: "Text is required");

            invalidPostException.AddData(
                key:nameof(Post.Author),
                values:"Author is required");

            invalidPostException.AddData(
                key: nameof(Post.CreatedDate),
                values: "Date is required");

            invalidPostException.AddData(
                key: nameof(Post.UpdatedDate),
                values: "Date is required");

            var expectedPostValidationException =
                new PostValidationException(invalidPostException);

            // when
            ValueTask<Post> modifyPostTask =
                this.postService.ModifyPostAsync(invalidPost);

            //then
            await Assert.ThrowsAsync<PostValidationException>(() =>
                modifyPostTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedPostValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePostAsync(It.IsAny<Post>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
