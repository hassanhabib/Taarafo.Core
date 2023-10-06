﻿
// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
		public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
		{
			// given
			Guid somePostId = Guid.NewGuid();

			var databaseUpdateConcurrencyException =
				new DbUpdateConcurrencyException();

			var lockedPostException =
				new LockedPostException(databaseUpdateConcurrencyException);

			var expectedPostDependencyValidationException =
				new PostDependencyValidationException(lockedPostException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectPostByIdAsync(It.IsAny<Guid>()))
					.ThrowsAsync(databaseUpdateConcurrencyException);

			// when
			ValueTask<Post> removePostByIdTask =
				this.postService.RemovePostByIdAsync(somePostId);

			PostDependencyValidationException actualPostDependencyValidationException =
				await Assert.ThrowsAsync<PostDependencyValidationException>(
					removePostByIdTask.AsTask);

			// then
			actualPostDependencyValidationException.Should().BeEquivalentTo(
				expectedPostDependencyValidationException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectPostByIdAsync(It.IsAny<Guid>()),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedPostDependencyValidationException))),
						Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
		{
			// given
			Guid somePostId = Guid.NewGuid();
			SqlException sqlException = GetSqlException();

			var failedPostStorageException =
				new FailedPostStorageException(sqlException);

			var expectedPostDependencyException =
				new PostDependencyException(failedPostStorageException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectPostByIdAsync(It.IsAny<Guid>()))
					.ThrowsAsync(sqlException);
			// when
			ValueTask<Post> deletePostTask =
				this.postService.RemovePostByIdAsync(somePostId);

			PostDependencyException actualPostDependencyException =
				await Assert.ThrowsAsync<PostDependencyException>(
					deletePostTask.AsTask);

			// then
			actualPostDependencyException.Should().BeEquivalentTo(
				expectedPostDependencyException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectPostByIdAsync(It.IsAny<Guid>()),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(
					expectedPostDependencyException))),
						Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
		{
			// given
			Guid somePostId = Guid.NewGuid();
			var serviceException = new Exception();

			var failedPostServiceException =
				new FailedPostServiceException(serviceException);

			var expectedPostServiceException =
				new PostServiceException(failedPostServiceException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectPostByIdAsync(It.IsAny<Guid>()))
					.ThrowsAsync(serviceException);

			// when
			ValueTask<Post> removePostByIdTask =
				this.postService.RemovePostByIdAsync(somePostId);

			PostServiceException actualPostServiceException =
				await Assert.ThrowsAsync<PostServiceException>(
					removePostByIdTask.AsTask);

			// then
			actualPostServiceException.Should().BeEquivalentTo(
				expectedPostServiceException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectPostByIdAsync(It.IsAny<Guid>()),
						Times.Once());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedPostServiceException))),
						Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}