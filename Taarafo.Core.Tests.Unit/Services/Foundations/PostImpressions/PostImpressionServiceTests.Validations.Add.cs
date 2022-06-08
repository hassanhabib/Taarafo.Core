// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Taarafo.Core.Models.Posts;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostImpressions
{
    public partial class PostImpressionServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfPostImpressionIsNullAndLogItAsync()
        {
            //given
            PostImpression nullPostImpression = null;

            var nullPostImpressionException =
                new NullPostImpressionException();

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(nullPostImpressionException);

            //when
            ValueTask<PostImpression> addPostImpressionTask =
                this.postImpressionService.AddPostImpressions(nullPostImpression);

            //then
            await Assert.ThrowsAsync<PostImpressionValidationException>(() =>
                addPostImpressionTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfPostImpressionIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidGuid = Guid.Empty;

            var invalidPostImpression = new PostImpression
            {
                PostId = invalidGuid,
                ProfileId = invalidGuid
            };

            var invalidPostImpressionException =
                new InvalidPostImpressionException();

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.PostId),
                values: "Id is required.");

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.Post),
                values: "Post is required.");

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.ProfileId),
                values: "Id is required.");

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.Profile),
                values: "Profile is required.");

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.CreatedDate),
                values: "Date is required.");

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.UpdatedDate),
                values: "Date is required.");

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(invalidPostImpressionException);

            //when
            ValueTask<PostImpression> addPostImpressionTask =
                this.postImpressionService.AddPostImpressions(invalidPostImpression);

            //then
            await Assert.ThrowsAsync<PostImpressionValidationException>(() =>
                addPostImpressionTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertPostImpressionAsync(invalidPostImpression),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
