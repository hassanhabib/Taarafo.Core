﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Posts
{
	public partial class PostServiceTests
	{
		[Fact]
		public async Task ShouldThrowValidationExceptionOnAddIfPostIsNullAndLogItAsync()
		{
			// given
			Post nullPost = null;

			var nullPostException =
				new NullPostException();

			var expectedPostValidationException =
				new PostValidationException(nullPostException);

			// when
			ValueTask<Post> addPostTask =
				this.postService.AddPostAsync(nullPost);

			PostValidationException actualPostValidationException =
			   await Assert.ThrowsAsync<PostValidationException>(
				   addPostTask.AsTask);

			// then
			actualPostValidationException.Should().BeEquivalentTo(
				expectedPostValidationException);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedPostValidationException))),
						Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		public async Task ShouldThrowValidationExceptionOnAddIfPostIsInvalidAndLogItAsync(
			string invalidText)
		{
			// given
			var invalidPost = new Post
			{
				Content = invalidText
			};

			var invalidPostException =
				new InvalidPostException();

			invalidPostException.AddData(
				key: nameof(Post.Id),
				values: "Id is required");

			invalidPostException.AddData(
				key: nameof(Post.Content),
				values: "Text is required");

			invalidPostException.AddData(
				key: nameof(Post.Author),
				values: "Text is required");

			invalidPostException.AddData(
				key: nameof(Post.CreatedDate),
				values: "Date is required");

			invalidPostException.AddData(
				key: nameof(Post.UpdatedDate),
				values: "Date is required");

			var expectedPostValidationException =
				new PostValidationException(invalidPostException);

			// when
			ValueTask<Post> addPostTask =
				this.postService.AddPostAsync(invalidPost);

			PostValidationException actualPostValidationException =
			   await Assert.ThrowsAsync<PostValidationException>(
				   addPostTask.AsTask);

			// then
			actualPostValidationException.Should().BeEquivalentTo(
				expectedPostValidationException);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedPostValidationException))),
						Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
		{
			// given
			DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
			int randomNumber = GetRandomNumber();
			Post randomPost = CreateRandomPost(randomDateTime);
			Post invalidPost = randomPost;

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Returns(randomDateTime);

			invalidPost.UpdatedDate =
				invalidPost.CreatedDate.AddDays(randomNumber);

			var invalidPostException =
				new InvalidPostException();

			invalidPostException.AddData(
				key: nameof(Post.UpdatedDate),
				values: $"Date is not the same as {nameof(Post.CreatedDate)}");

			var expectedPostValidationException =
				new PostValidationException(invalidPostException);

			// when
			ValueTask<Post> addPostTask =
				this.postService.AddPostAsync(invalidPost);

			PostValidationException actualPostValidationException =
				await Assert.ThrowsAsync<PostValidationException>(
					addPostTask.AsTask);

			// then
			actualPostValidationException.Should().BeEquivalentTo(
				expectedPostValidationException);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedPostValidationException))),
						Times.Once);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
						Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
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

			Post randomPost = CreateRandomPost(invalidDateTime);
			Post invalidPost = randomPost;

			var invalidPostException =
				new InvalidPostException();

			invalidPostException.AddData(
				key: nameof(Post.CreatedDate),
				values: "Date is not recent");

			var expectedPostValidationException =
				new PostValidationException(invalidPostException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Returns(randomDateTime);

			// when
			ValueTask<Post> addPostTask =
				this.postService.AddPostAsync(invalidPost);

			PostValidationException actualPostValidationException =
			   await Assert.ThrowsAsync<PostValidationException>(
				   addPostTask.AsTask);

			// then
			actualPostValidationException.Should().BeEquivalentTo(
				expectedPostValidationException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedPostValidationException))),
						Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}
