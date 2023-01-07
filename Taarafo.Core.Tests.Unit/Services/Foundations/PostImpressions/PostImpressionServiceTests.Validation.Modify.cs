// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostImpressions
{
    public partial class PostImpressionServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfPostImpressionIsNullAndLogItAsync()
        {
            //given
            PostImpression nullPostImpression = null;
            var nullPostImpressionException = new NullPostImpressionException();

            PostImpressionValidationException expectedPostValidationException =
                new PostImpressionValidationException(nullPostImpressionException);

            //when
            ValueTask<PostImpression> modifyPostImpressionTask =
                this.postImpressionService.ModifyPostImpressionAsync(nullPostImpression);

            PostImpressionValidationException actualPostImpressionValidationException =
                await Assert.ThrowsAsync<PostImpressionValidationException>(
                    modifyPostImpressionTask.AsTask);

            //then
            actualPostImpressionValidationException.Should().BeEquivalentTo(
                expectedPostValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedPostValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePostImpressionAsync(nullPostImpression), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        public async Task ShouldThrowValidationExceptionOnModifyIfPostImpressionIsInvalidAndLogItAsync(Guid invalidId)
        {
            //given
            var invalidPostImpression = new PostImpression
            {
                PostId = invalidId,
                ProfileId = invalidId
            };

            var invalidPostImpressionException = new InvalidPostImpressionException();

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.PostId),
                values: "Id is required");

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.ProfileId),
                values: "Id is required");

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.CreatedDate),
                values: "Date is required");

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.UpdatedDate),
                values: "Date is required");

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(invalidPostImpressionException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(GetRandomDateTime);

            //when
            ValueTask<PostImpression> modifyPostImpressionTask =
                this.postImpressionService.ModifyPostImpressionAsync(invalidPostImpression);

            PostImpressionValidationException actualPostValidationException =
                await Assert.ThrowsAsync<PostImpressionValidationException>(
                    modifyPostImpressionTask.AsTask);

            //then
            actualPostValidationException.Should().BeEquivalentTo(
                expectedPostImpressionValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdatePostImpressionAsync(invalidPostImpression), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
