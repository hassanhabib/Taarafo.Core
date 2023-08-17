﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Posts
{
    public partial class PostServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedPostStorageException =
                new FailedPostStorageException(
                    message: "Failed post storage error occurred, contact support.",
                        innerException: sqlException);

            var expectedPostDependencyException =
                new PostDependencyException(
                    message: "Post dependency error occurred, contact support.",
                        innerException: failedPostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Post> retrievePostByIdTask =
                this.postService.RetrievePostByIdAsync(someId);

            PostDependencyException actaulPostDependencyException =
                await Assert.ThrowsAsync<PostDependencyException>(
                    retrievePostByIdTask.AsTask);

            // then
            actaulPostDependencyException.Should().BeEquivalentTo(
                expectedPostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnRetrieveByIdIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedPostServiceException =
                new FailedPostServiceException(
                    message: "Failed post service occurred, please contact support",
                        innerException: serviceException);

            var expectedPostServiceException =
                new PostServiceException(
                    message: "Post service error occurred, contact support.",
                        innerException: failedPostServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Post> retrievePostByIdTask =
                this.postService.RetrievePostByIdAsync(someId);

            PostServiceException actualPostServiceException =
                await Assert.ThrowsAsync<PostServiceException>(
                    retrievePostByIdTask.AsTask);

            // then
            actualPostServiceException.Should().BeEquivalentTo(expectedPostServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedPostServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}