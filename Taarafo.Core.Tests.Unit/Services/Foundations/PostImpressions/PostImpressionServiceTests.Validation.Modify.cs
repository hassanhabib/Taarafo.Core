// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
                broker.LogError(It.Is(SameExceptionAs(expectedPostValidationException))), 
                  Times.Once);

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
                 values: new[]
                    {
                        "Date is required",
                        $"Date is the same as {nameof(PostImpression.CreatedDate)}"
                    });

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(invalidPostImpressionException);

            //when
            ValueTask<PostImpression> modifyPostImpressionTask =
                this.postImpressionService.ModifyPostImpressionAsync(invalidPostImpression);

            PostImpressionValidationException actualPostValidationException =
                await Assert.ThrowsAsync<PostImpressionValidationException>(
                    modifyPostImpressionTask.AsTask);

            //then
            actualPostValidationException.Should().BeEquivalentTo(
                expectedPostImpressionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), 
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))),  
                      Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            PostImpression randomPostImpression = CreateRandomPostImpression(randomDateTime);
            PostImpression invalidPostImpression = randomPostImpression;
            var invalidPostImpressionException = new InvalidPostImpressionException();

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.UpdatedDate),
                values: $"Date is the same as {nameof(PostImpression.CreatedDate)}");

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(invalidPostImpressionException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            //when
            ValueTask<PostImpression> modifyPostImpressionTask =
                this.postImpressionService.ModifyPostImpressionAsync(invalidPostImpression);

            PostImpressionValidationException actualPostImpressionValidationException =
                await Assert.ThrowsAsync<PostImpressionValidationException>(modifyPostImpressionTask.AsTask);

            //then
            actualPostImpressionValidationException.Should().BeEquivalentTo(
                expectedPostImpressionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), 
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))), 
                      Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minuts)
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            PostImpression randomPostImpression = CreateRandomPostImpression(dateTime);
            PostImpression inputPostImpression = randomPostImpression;
            inputPostImpression.UpdatedDate = dateTime.AddMinutes(minuts);
            var invalidPostImpressionException = new InvalidPostImpressionException();

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.UpdatedDate),
                values: "Date is not recent");

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(invalidPostImpressionException);

            dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(dateTime);

            //when
            ValueTask<PostImpression> modifyPostImpressionTask =
                this.postImpressionService.ModifyPostImpressionAsync(inputPostImpression);

            PostImpressionValidationException actualPostImpressionValidationException =
                await Assert.ThrowsAsync<PostImpressionValidationException>(
                    modifyPostImpressionTask.AsTask);

            //then
            actualPostImpressionValidationException.Should().BeEquivalentTo(
                expectedPostImpressionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), 
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))), 
                      Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfPostImpressionDoesNotExistAndLogItAsync()
        {
            //given
            int randomNegativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            PostImpression randomPostImpression = CreateRandomPostImpression(dateTime);
            PostImpression nonExistPostImpression = randomPostImpression;
            nonExistPostImpression.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            PostImpression nullPostImpression = null;

            var notFoundPostImpressionException =
                new NotFoundPostImpressionException(
                    nonExistPostImpression.PostId,
                    nonExistPostImpression.ProfileId);

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(notFoundPostImpressionException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(nonExistPostImpression.PostId,
                    nonExistPostImpression.ProfileId)).ReturnsAsync(nullPostImpression);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(dateTime);

            //when
            ValueTask<PostImpression> modifyPostImpressionTask =
                this.postImpressionService.ModifyPostImpressionAsync(nonExistPostImpression);

            PostImpressionValidationException actualPostImpressionValidationException =
                await Assert.ThrowsAsync<PostImpressionValidationException>(modifyPostImpressionTask.AsTask);

            //then
            actualPostImpressionValidationException.Should().BeEquivalentTo(
                expectedPostImpressionValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(
                    nonExistPostImpression.PostId,
                    nonExistPostImpression.ProfileId), 
                      Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), 
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))), 
                      Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            //given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            PostImpression randomPostImpression = CreateRandomModifyPostImpression(randomDateTime);
            PostImpression invalidPostImpression = randomPostImpression.DeepClone();
            PostImpression storagePostImpression = randomPostImpression.DeepClone();
            storagePostImpression.CreatedDate = storagePostImpression.CreatedDate.AddMinutes(randomMinutes);
            storagePostImpression.UpdatedDate = storagePostImpression.UpdatedDate.AddMinutes(randomMinutes);
            Guid postId = invalidPostImpression.PostId;
            Guid profileId = invalidPostImpression.ProfileId;

            var invalidPostImpressionException =
                new InvalidPostImpressionException();

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.CreatedDate),
                values: $"Date is not the same as {nameof(PostImpression.CreatedDate)}");

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(invalidPostImpressionException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(postId, profileId))
                    .ReturnsAsync(storagePostImpression);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            //when
            ValueTask<PostImpression> modifyPostImpressionTask =
                this.postImpressionService.ModifyPostImpressionAsync(invalidPostImpression);

            PostImpressionValidationException actualPostImpressionValidationException =
                await Assert.ThrowsAsync<PostImpressionValidationException>(modifyPostImpressionTask.AsTask);

            //then
            actualPostImpressionValidationException.Should().BeEquivalentTo(
                expectedPostImpressionValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(invalidPostImpression.PostId,
                    invalidPostImpression.ProfileId), 
                      Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), 
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedPostImpressionValidationException))), 
                     Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            PostImpression randomPostImpression = CreateRandomModifyPostImpression(randomDateTime);
            PostImpression invalidPostImpression = randomPostImpression;
            PostImpression storagePostImpression = invalidPostImpression.DeepClone();
            invalidPostImpression.UpdatedDate = storagePostImpression.UpdatedDate;
            var invalidPostImpressionException = new InvalidPostImpressionException();

            invalidPostImpressionException.AddData(
                key: nameof(PostImpression.UpdatedDate),
                values: $"Date is the same as {nameof(PostImpression.UpdatedDate)}");

            var expectedPostImpressionValidationException =
                new PostImpressionValidationException(invalidPostImpressionException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPostImpressionByIdAsync(
                    invalidPostImpression.PostId, invalidPostImpression.ProfileId)).ReturnsAsync(storagePostImpression);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            //when
            ValueTask<PostImpression> modifyPostImpressionTask =
                this.postImpressionService.ModifyPostImpressionAsync(invalidPostImpression);

            PostImpressionValidationException actualPostImpressionValidationException =
                await Assert.ThrowsAsync<PostImpressionValidationException>(modifyPostImpressionTask.AsTask);

            //then
            actualPostImpressionValidationException.Should().BeEquivalentTo(
                expectedPostImpressionValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), 
                  Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPostImpressionValidationException))), 
                      Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPostImpressionByIdAsync(
                    invalidPostImpression.PostId, invalidPostImpression.ProfileId), 
                      Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
