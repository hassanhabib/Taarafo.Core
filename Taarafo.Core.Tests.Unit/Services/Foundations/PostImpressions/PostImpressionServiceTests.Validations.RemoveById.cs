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
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenProfileIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomPostId= Guid.NewGuid();
            Guid randomProfileId= default;
            Guid inputPostId = randomPostId;
            Guid inputProfileId= randomProfileId;

            var invalidPostImpressionException = new InvalidPostImpressionException(
                    parameterName:nameof(PostImpression.ProfileId),
                    parameterValue:inputProfileId);

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(invalidPostImpressionException);

            //when
            ValueTask<PostImpression> removePostImpressionTask= 
                    this.postImpressionService.RemovePostImpressionByIdAsync(inputPostId, inputProfileId);

            //then
            await Assert.ThrowsAsync<PostImpressionValidationException>(()=>
                removePostImpressionTask.AsTask());

            this.loggingBrokerMock.Verify(broker=>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))),Times.Once);

            this.storageBrokerMock.Verify(broker=>
                broker.SelectPostImpressionByIdsAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()),
                        Times.Never);

            this.storageBrokerMock.Verify(broker=>
                broker.DeletePostImpressionAsync(
                    It.IsAny<PostImpression>()),Times.Never);
            
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
