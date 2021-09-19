// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Posts
{
    public partial class PostServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfPostIsNullAndLogItAsync()
        {
            // given
            Post nullPost = null;

            var nullPostException =
                new NullPostException();

            var expectedPostValidationException =
                new PostValidationException(nullPostException);

            // when
            ValueTask<Post> addPostTask =
                this.postService.AddPostAsync(nullPost);

            // then
            await Assert.ThrowsAsync<PostValidationException>(() =>
                addPostTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedPostValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfPostIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidPost = new Post
            {
                Content = invalidText
            };

            var invalidPostException =
                new InvalidPostException();

            invalidPostException.AddData(
                key: nameof(Post.Id),
                values: "Id is required");

            invalidPostException.AddData(
                key: nameof(Post.Content),
                values: "Text is required");

            invalidPostException.AddData(
                key: nameof(Post.Author),
                values: "Id is required");

            invalidPostException.AddData(
                key: nameof(Post.CreatedDate),
                values: "Date is required");

            invalidPostException.AddData(
                key: nameof(Post.UpdatedDate),
                values: "Date is required");

            var expectedPostValidationException =
                new PostValidationException(invalidPostException);

            // when
            ValueTask<Post> addPostTask =
                this.postService.AddPostAsync(invalidPost);

            // then
            await Assert.ThrowsAsync<PostValidationException>(() =>
               addPostTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedPostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostAsync(It.IsAny<Post>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogitAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            Post randomPost = CreateRandomPost();
            Post invalidPost = randomPost;
            
            invalidPost.UpdatedDate = 
                invalidPost.CreatedDate.AddDays(randomNumber);

            var invalidPostException =
                new InvalidPostException();

            invalidPostException.AddData(
                key: nameof(Post.UpdatedDate),
                values: $"Date is not the same as {nameof(Post.CreatedDate)}");

            var expectedPostValidationException =
                new PostValidationException(invalidPostException);

            // when
            ValueTask<Post> addPostTask =
                this.postService.AddPostAsync(invalidPost);

            // then
            await Assert.ThrowsAsync<PostValidationException>(() =>
               addPostTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedPostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostAsync(It.IsAny<Post>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
