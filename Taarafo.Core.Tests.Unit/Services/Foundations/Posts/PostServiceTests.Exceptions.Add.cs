// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Posts
{
    public partial class PostServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDepdnencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Post somePost = CreateRandomPost();
            SqlException sqlException = GetSqlException();

            var failedPostStorageException =
                new FailedPostStorageException(sqlException);

            var expectedPostDependencyException =
                new PostDependencyException(failedPostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPostAsync(It.IsAny<Post>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Post> addPostTask =
                this.postService.AddPostAsync(somePost);

            // then
            await Assert.ThrowsAsync<PostDependencyException>(() =>
               addPostTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostAsync(It.IsAny<Post>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfPostAlreadyExistsAndLogItAsync()
        {
            // given
            Post randomPost = CreateRandomPost();
            Post alreadyExistsPost = randomPost;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsPostException =
                new AlreadyExsitPostException(duplicateKeyException);

            var expectedPostDependencyValidationException =
                new PostDependencyValidationException(alreadyExistsPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPostAsync(It.IsAny<Post>()))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Post> addPostTask =
                this.postService.AddPostAsync(alreadyExistsPost);

            // then
            await Assert.ThrowsAsync<PostDependencyValidationException>(() =>
                addPostTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostAsync(It.IsAny<Post>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedPostDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Post somePost = CreateRandomPost();

            var databaseUpdateException =
                new DbUpdateException();

            var failedPostStorageException =
                new FailedPostStorageException(databaseUpdateException);

            var expectedPostDependencyException =
                new PostDependencyException(failedPostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPostAsync(It.IsAny<Post>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Post> addPostTask =
                this.postService.AddPostAsync(somePost);

            // then
            await Assert.ThrowsAsync<PostDependencyException>(() =>
               addPostTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostAsync(It.IsAny<Post>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Post somePost = CreateRandomPost();
            var serviceException = new Exception();

            var failedPostServiceException =
                new FailedPostServiceException(serviceException);

            var expectedPostServiceException =
                new PostServiceException(failedPostServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertPostAsync(It.IsAny<Post>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Post> addPostTask =
                this.postService.AddPostAsync(somePost);

            // then
            await Assert.ThrowsAsync<PostServiceException>(() =>
                addPostTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostAsync(It.IsAny<Post>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}