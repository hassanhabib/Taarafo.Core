// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
    }
}
