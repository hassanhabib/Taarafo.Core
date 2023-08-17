// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.Posts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Posts
{
    public partial class PostServiceTests
    {
        [Fact]
        private void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedPostStorageException(
                    message: "Failed post storage error occurred, contact support.",
                        innerException: sqlException);

            var expectedPostDependencyException =
                new PostDependencyException(
                    message: "Post dependency error occurred, contact support.",
                        innerException: failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPosts())
                    .Throws(sqlException);

            // when
            Action retrieveAllPostsAction = () =>
                this.postService.RetrieveAllPosts();

            PostDependencyException actualPostDependencyException =
                Assert.Throws<PostDependencyException>(
                    retrieveAllPostsAction);

            // then
            actualPostDependencyException.Should().BeEquivalentTo(
                expectedPostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPosts(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedPostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedPostServiceException =
                new FailedPostServiceException(
                    message: "Failed post service occurred, please contact support",
                        innerException: serviceException);

            var expectedPostServiceException =
                new PostServiceException(
                    message: "Post service error occurred, contact support.",
                        innerException: failedPostServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPosts())
                    .Throws(serviceException);

            // when
            Action retrieveAllPostsAction = () =>
                this.postService.RetrieveAllPosts();

            PostServiceException actualPostServiceException =
                Assert.Throws<PostServiceException>(
                    retrieveAllPostsAction);

            // then
            actualPostServiceException.Should().BeEquivalentTo(expectedPostServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPosts(),
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