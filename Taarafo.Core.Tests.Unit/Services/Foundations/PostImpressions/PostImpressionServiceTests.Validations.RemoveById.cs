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
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostImpressions
{
    public partial class PostImpressionServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidPostImpressionId = Guid.Empty;

            var invalidPostImpressionException =
                new InvalidPostImpressionException();

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.PostId),
                values: "Id is required");

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.ProfileId),
                values: "Id is required");

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(invalidPostImpressionException);

            //when
            ValueTask<PostImpression> removePostImpressionByIdTask =
                this.postImpressionService.RemovePostImpressionByIdAsync(invalidPostImpressionId);

            //then
            await Assert.ThrowsAsync<PostImpressionValidationException>(() =>
                removePostImpressionByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePostImpressionAsync(It.IsAny<PostImpression>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfPostImpressionIsNotFoundAndLogItAsync()
        {
            //given
            Guid randomPostImpressionId = Guid.NewGuid();
            Guid inputPostImpressionId = randomPostImpressionId;
            PostImpression noPostImpression = null;

            var notFoundPostImpressionException =
                new NotFoundPostImpressionException(inputPostImpressionId);

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(
                    notFoundPostImpressionException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noPostImpression);

            //when
            ValueTask<PostImpression> removePostImpressionByIdTask =
                this.postImpressionService.RemovePostImpressionByIdAsync(inputPostImpressionId);

            //then
            await Assert.ThrowsAsync<PostImpressionValidationException>(() =>
                removePostImpressionByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePostImpressionAsync(It.IsAny<PostImpression>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
