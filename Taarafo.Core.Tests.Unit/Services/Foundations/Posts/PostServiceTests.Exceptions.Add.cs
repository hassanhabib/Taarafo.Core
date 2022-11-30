// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Taarafo.Core.Models.Posts;
using Taarafo.Core.Models.Posts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Posts
{
	public partial class PostServiceTests
	{
		[Fact]
		public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
		{
			// given
			Post somePost = CreateRandomPost();
			SqlException sqlException = GetSqlException();

			var failedPostStorageException =
				new FailedPostStorageException(sqlException);

			var expectedPostDependencyException =
				new PostDependencyException(failedPostStorageException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(sqlException);

			// when
			ValueTask<Post> addPostTask =
				this.postService.AddPostAsync(somePost);

			PostDependencyException actualPostDependencyException =
			   await Assert.ThrowsAsync<PostDependencyException>(
				   addPostTask.AsTask);

			// then
			actualPostDependencyException.Should().BeEquivalentTo(
				expectedPostDependencyException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertPostAsync(It.IsAny<Post>()),
					Times.Never);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(
					expectedPostDependencyException))),
						Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowDependencyValidationExceptionOnAddIfPostAlreadyExsitsAndLogItAsync()
		{
			// given
			Post randomPost = CreateRandomPost();
			Post alreadyExistsPost = randomPost;
			string randomMessage = GetRandomMessage();

			var duplicateKeyException =
				new DuplicateKeyException(randomMessage);

			var alreadyExistsPostException =
				new AlreadyExistsPostException(duplicateKeyException);

			var expectedPostDependencyValidationException =
				new PostDependencyValidationException(alreadyExistsPostException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(duplicateKeyException);

			// when
			ValueTask<Post> addPostTask =
				this.postService.AddPostAsync(alreadyExistsPost);

			PostDependencyValidationException actualPostDependencyValidationException =
			   await Assert.ThrowsAsync<PostDependencyValidationException>(
				   addPostTask.AsTask);

			// then
			actualPostDependencyValidationException.Should().BeEquivalentTo(
				expectedPostDependencyValidationException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertPostAsync(It.IsAny<Post>()),
					Times.Never);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedPostDependencyValidationException))),
						Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
		{
			// given
			Post somePost = CreateRandomPost();

			var databaseUpdateException =
				new DbUpdateException();

			var failedPostStorageException =
				new FailedPostStorageException(databaseUpdateException);

			var expectedPostDependencyException =
				new PostDependencyException(failedPostStorageException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(databaseUpdateException);

			// when
			ValueTask<Post> addPostTask =
				this.postService.AddPostAsync(somePost);

			PostDependencyException actualPostDependencyException =
			  await Assert.ThrowsAsync<PostDependencyException>(
				  addPostTask.AsTask);

			// then
			actualPostDependencyException.Should().BeEquivalentTo(
				expectedPostDependencyException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedPostDependencyException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertPostAsync(It.IsAny<Post>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowServiceExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
		{
			// given
			Post somePost = CreateRandomPost();
			var serviceException = new Exception();

			var failedPostServiceException =
				new FailedPostServiceException(serviceException);

			var expectedPostServiceException =
				new PostServiceException(failedPostServiceException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(serviceException);

			// when
			ValueTask<Post> addPostTask =
				this.postService.AddPostAsync(somePost);

			PostServiceException actualPostServiceException =
			  await Assert.ThrowsAsync<PostServiceException>(
				  addPostTask.AsTask);

			// then
			actualPostServiceException.Should().BeEquivalentTo(
				expectedPostServiceException);

			// then
			await Assert.ThrowsAsync<PostServiceException>(() =>
				addPostTask.AsTask());

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertPostAsync(It.IsAny<Post>()),
					Times.Never);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedPostServiceException))),
						Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}