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
    }
}
