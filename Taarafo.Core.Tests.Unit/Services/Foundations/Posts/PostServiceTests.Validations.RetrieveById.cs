// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfPostIsNotFoundAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            Post emptyPost = null;

            var notFoundPostException =
                new NotFoundPostException(someId);

            var expectedPostValidationException =
                new PostValidationException(notFoundPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(someId))
                    .ReturnsAsync(emptyPost);

            //when
            ValueTask<Post> retrievePostTask =
                this.postService.RetrievePostByIdAsync(someId);

            //then
            await Assert.ThrowsAsync<PostValidationException>(() =>
               retrievePostTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
            broker.SelectPostByIdAsync(someId),
                Times.Once());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
