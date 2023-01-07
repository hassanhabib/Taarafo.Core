// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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
    }
}
