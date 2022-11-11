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
using Taarafo.Core.Models.Posts.Exceptions;
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

			PostImpressionValidationException actualPostImpressionValidationException =
				await Assert.ThrowsAsync<PostImpressionValidationException>(
					addPostImpressionTask.AsTask);

			//then
			actualPostImpressionValidationException.Should().BeEquivalentTo(
				expectedPostImpressionValidationException);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedPostImpressionValidationException))),
						Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
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
				values: "Id is required");

			invalidPostImpressionException.AddData(
				key: nameof(PostImpression.Post),
				values: "Object is required");

			invalidPostImpressionException.AddData(
				key: nameof(PostImpression.ProfileId),
				values: "Id is required");

			invalidPostImpressionException.AddData(
				key: nameof(PostImpression.Profile),
				values: "Object is required");

			invalidPostImpressionException.AddData(
				key: nameof(PostImpression.CreatedDate),
				values: "Date is required");

			invalidPostImpressionException.AddData(
				key: nameof(PostImpression.UpdatedDate),
				values: "Date is required");

			var expectedPostImpressionValidationException =
				new PostImpressionValidationException(invalidPostImpressionException);

			//when
			ValueTask<PostImpression> addPostImpressionTask =
				this.postImpressionService.AddPostImpressions(invalidPostImpression);

			PostImpressionValidationException actualPostImpressionValidationException =
				await Assert.ThrowsAsync<PostImpressionValidationException>(
					addPostImpressionTask.AsTask);

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
				broker.InsertPostImpressionAsync(invalidPostImpression),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
		{
			//given
			int randomNumber = GetRandomNumber();
			DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
			PostImpression randomPostImpression = CreateRandomPostImpression(randomDateTimeOffset);
			PostImpression invalidPostImpression = randomPostImpression;

			invalidPostImpression.UpdatedDate =
				invalidPostImpression.CreatedDate.AddDays(randomNumber);

			var invalidPostImpressionException =
				new InvalidPostImpressionException();

			invalidPostImpressionException.AddData(
				key: nameof(PostImpression.UpdatedDate),
				values: $"Date is not the same as {nameof(PostImpression.CreatedDate)}");

			var expectedPostImpressionValidationException =
				new PostImpressionValidationException(invalidPostImpressionException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Returns(randomDateTimeOffset);

			//when
			ValueTask<PostImpression> addPostImpressionTask =
				this.postImpressionService.AddPostImpressions(invalidPostImpression);

			PostImpressionValidationException actualPostImpressionValidationException =
				await Assert.ThrowsAsync<PostImpressionValidationException>(
					addPostImpressionTask.AsTask);

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
				broker.InsertPostImpressionAsync(It.IsAny<PostImpression>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}


		[Theory]
		[MemberData(nameof(MinutesBeforeOrAfter))]
		public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
			int minutesBeforeOrAfter)
		{
			// given
			DateTimeOffset randomDateTime =
				GetRandomDateTimeOffset();

			DateTimeOffset invalidDateTime =
				randomDateTime.AddMinutes(minutesBeforeOrAfter);

			PostImpression randomPostImpression = CreateRandomPostImpression(invalidDateTime);
			PostImpression invalidPostImpression = randomPostImpression;

			var invalidPostException =
				new InvalidPostImpressionException();

			invalidPostException.AddData(
				key: nameof(PostImpression.CreatedDate),
				values: "Date is not recent");

			var expectedPostImpressionValidationException =
				new PostImpressionValidationException(invalidPostException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Returns(randomDateTime);

			// when
			ValueTask<PostImpression> addPostImpressionTask =
				this.postImpressionService.AddPostImpressions(invalidPostImpression);

			PostImpressionValidationException actualPostImpressionValidationException =
			   await Assert.ThrowsAsync<PostImpressionValidationException>(
				   addPostImpressionTask.AsTask);

			// then
			actualPostImpressionValidationException.Should().BeEquivalentTo(
				expectedPostImpressionValidationException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedPostImpressionValidationException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertPostImpressionAsync(It.IsAny<PostImpression>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}
