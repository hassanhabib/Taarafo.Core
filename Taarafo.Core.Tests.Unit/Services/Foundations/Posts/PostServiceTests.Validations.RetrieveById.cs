// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfPostIsNullAndLogItAsync()
        {
            //given
            Guid postId = Guid.NewGuid();
            Post invalidPost = null;

            var notFoundPostException =
                new NotFoundPostException(postId);

            var expectedPostValidationException =
                new PostValidationException(notFoundPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostByIdAsync(postId))
                    .ReturnsAsync(invalidPost);

            //when
            ValueTask<Post> retrievePostTask =
                this.postService.RetrievePostByIdAsync(postId);

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
            broker.SelectPostByIdAsync(postId), 
                Times.Once());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
