// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
        private async Task ShouldThrowValidationExceptionOnModifyIfPostIsNullAndLogItAsync()
        {
            // given
            Post nullPost = null;
            var nullPostException = new NullPostException();

            var expectedPostValidationException =
                new PostValidationException(
                    message: "Post validation errors occurred, please try again.",
                        innerException: nullPostException);

            // when
            ValueTask<Post> modifyPostTask =
                this.postService.ModifyPostAsync(nullPost);

            PostValidationException actualPostValidationException =
                await Assert.ThrowsAsync<PostValidationException>(
                    modifyPostTask.AsTask);

            // then
            actualPostValidationException.Should().BeEquivalentTo(expectedPostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostValidationException))),
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
        [InlineData(" ")]
        private async Task ShouldThrowValidationExceptionOnModifyIfPostIsInvalidAndLogItAsync(string invalidText)
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
                key: nameof(Post.Author),
                values: "Text is required");

            invalidPostException.AddData(
                key: nameof(Post.CreatedDate),
                values: "Date is required");

            invalidPostException.AddData(
                key: nameof(Post.UpdatedDate),
                "Date is required",
                $"Date is the same as {nameof(Post.CreatedDate)}");

            var expectedPostValidationException =
                new PostValidationException(
                    message: "Post validation errors occurred, please try again.",
                        innerException: invalidPostException);

            // when
            ValueTask<Post> modifyPostTask =
                this.postService.ModifyPostAsync(invalidPost);

            PostValidationException actualPostValidationException =
                await Assert.ThrowsAsync<PostValidationException>(modifyPostTask.AsTask);

            // then
            actualPostValidationException.Should().BeEquivalentTo(expectedPostValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePostAsync(It.IsAny<Post>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Post randomPost = CreateRandomPost(randomDateTime);
            Post invalidPost = randomPost;
            var invalidPostException = new InvalidPostException();

            invalidPostException.AddData(
                key: nameof(Post.UpdatedDate),
                values: $"Date is the same as {nameof(Post.CreatedDate)}");

            var expectedPostValidationException =
                new PostValidationException(
                    message: "Post validation errors occurred, please try again.",
                        innerException: invalidPostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Post> modifyPostTask =
                this.postService.ModifyPostAsync(invalidPost);

            PostValidationException actualPostValidationException =
                await Assert.ThrowsAsync<PostValidationException>(
                    modifyPostTask.AsTask);

            // then
            actualPostValidationException.Should().BeEquivalentTo(expectedPostValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(invalidPost.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        private async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            Post randomPost = CreateRandomPost(dateTime);
            Post inputPost = randomPost;
            inputPost.UpdatedDate = dateTime.AddMinutes(minutes);
            var invalidPostException =
                new InvalidPostException();

            invalidPostException.AddData(
                key: nameof(Post.UpdatedDate),
                values: "Date is not recent");

            var expectedPostValidationException =
                new PostValidationException(
                    message: "Post validation errors occurred, please try again.",
                        innerException: invalidPostException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(dateTime);

            // when
            ValueTask<Post> modifyPostTask =
                this.postService.ModifyPostAsync(inputPost);

            PostValidationException actualPostValidationException =
                await Assert.ThrowsAsync<PostValidationException>(
                    modifyPostTask.AsTask);

            // then
            actualPostValidationException.Should().BeEquivalentTo(expectedPostValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfPostDoesNotExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            Post randomPost = CreateRandomPost(dateTime);
            Post nonExistPost = randomPost;
            nonExistPost.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Post nullPost = null;

            var notFoundPostException =
                new NotFoundPostException(nonExistPost.Id);

            var expectedPostValidationException =
                new PostValidationException(
                    message: "Post validation errors occurred, please try again.",
                        innerException: notFoundPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(nonExistPost.Id))
                .ReturnsAsync(nullPost);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(dateTime);

            // when 
            ValueTask<Post> modifyPostTask =
                this.postService.ModifyPostAsync(nonExistPost);

            PostValidationException actualPostValidationException =
               await Assert.ThrowsAsync<PostValidationException>(
                   modifyPostTask.AsTask);

            // then
            actualPostValidationException.Should().BeEquivalentTo(
                expectedPostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(nonExistPost.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Post randomPost = CreateRandomModifyPost(randomDateTimeOffset);
            Post invalidPost = randomPost.DeepClone();
            Post storagePost = invalidPost.DeepClone();

            storagePost.CreatedDate =
                storagePost.CreatedDate.AddMinutes(randomMinutes);

            storagePost.UpdatedDate =
                storagePost.UpdatedDate.AddMinutes(randomMinutes);

            var invalidPostException = new InvalidPostException();
            Guid postId = invalidPost.Id;

            invalidPostException.AddData(
                key: nameof(Post.CreatedDate),
                values: $"Date is not the same as {nameof(Post.CreatedDate)}");

            var expectedPostValidationException =
                new PostValidationException(
                    message: "Post validation errors occurred, please try again.",
                        innerException: invalidPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(postId))
                .ReturnsAsync(storagePost);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<Post> modifyPostTask =
                this.postService.ModifyPostAsync(invalidPost);

            PostValidationException actualPostValidationException =
                await Assert.ThrowsAsync<PostValidationException>(
                    modifyPostTask.AsTask);

            // then
            actualPostValidationException.Should().BeEquivalentTo(
                expectedPostValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(invalidPost.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedPostValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Post randomPost = CreateRandomModifyPost(randomDateTime);
            Post invalidPost = randomPost;
            Post storagePost = randomPost.DeepClone();
            invalidPost.UpdatedDate = storagePost.UpdatedDate;
            Guid postId = invalidPost.Id;
            var invalidPostException = new InvalidPostException();

            invalidPostException.AddData(
                key: nameof(Post.UpdatedDate),
                values: $"Date is the same as {nameof(Post.UpdatedDate)}");

            var expectedPostValidationException =
                new PostValidationException(
                    message: "Post validation errors occurred, please try again.",
                        innerException: invalidPostException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectPostByIdAsync(invalidPost.Id))
               .ReturnsAsync(storagePost);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Post> modifyPostTask =
                this.postService.ModifyPostAsync(invalidPost);

            PostValidationException actualPostValidationException =
               await Assert.ThrowsAsync<PostValidationException>(
                   modifyPostTask.AsTask);

            // then
            actualPostValidationException.Should().BeEquivalentTo(
                expectedPostValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(postId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}