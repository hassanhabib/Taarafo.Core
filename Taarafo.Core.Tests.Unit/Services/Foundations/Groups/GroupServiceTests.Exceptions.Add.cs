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
using Taarafo.Core.Models.Groups;
using Taarafo.Core.Models.Groups.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Groups
{
	public partial class GroupServiceTests
	{
		[Fact]
		public async Task ShouldThrowCriticalDependencyExceptionOnCreateIfSqlErrorOccursAndLogItAsync()
		{
			// given
			DateTimeOffset randomDateTime = GetRandomDateTime();
			Group someGroup = CreateRandomGroup(randomDateTime);
			SqlException sqlException = GetSqlException();

			var failedGroupStorageException =
				new FailedGroupStorageException(sqlException);

			var expectedGroupDependencyException =
				new GroupDependencyException(failedGroupStorageException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(sqlException);

			// when
			ValueTask<Group> addGroupTask =
				this.groupService.AddGroupAsync(someGroup);

			// then
			GroupDependencyException actualGroupDependencyException = 
				 await Assert.ThrowsAsync<GroupDependencyException>(() =>
					addGroupTask.AsTask());

			actualGroupDependencyException.Should().BeEquivalentTo(
				expectedGroupDependencyException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(
					expectedGroupDependencyException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertGroupAsync(It.IsAny<Group>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowDependencyValidationExceptionOnCreateIfGroupAlreadyExsitsAndLogItAsync()
		{
			// given
			Group randomGroup = CreateRandomGroup();
			Group alreadyExistsGroup = randomGroup;
			string randomMessage = GetRandomMessage();

			var duplicateKeyException =
				new DuplicateKeyException(randomMessage);

			var alreadyExistsGroupException =
				new AlreadyExistsGroupException(duplicateKeyException);

			var expectedGroupDependencyException =
				new GroupDependencyException(alreadyExistsGroupException);

			this.dateTimeBrokerMock.Setup(broker =>
			  broker.GetCurrentDateTimeOffset())
				  .Throws(duplicateKeyException);

			// when
			ValueTask<Group> addGroupTask =
				this.groupService.AddGroupAsync(alreadyExistsGroup);

			// then
			GroupDependencyException actualGroupDependencyException = 
				 await Assert.ThrowsAsync<GroupDependencyException>(() =>
					addGroupTask.AsTask());

			actualGroupDependencyException.Should().BeEquivalentTo(
				expectedGroupDependencyException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertGroupAsync(It.IsAny<Group>()),
					Times.Never);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedGroupDependencyException))),
						Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async void ShouldThrowValidationExceptionOnCreateIfReferenceErrorOccursAndLogItAsync()
		{
			// given
			Group someGroup = CreateRandomGroup();
			string randomMessage = GetRandomMessage();
			string exceptionMessage = randomMessage;

			var foreignKeyConstraintConflictException =
				new ForeignKeyConstraintConflictException(exceptionMessage);

			var invalidGroupReferenceException =
				new InvalidGroupReferenceException(foreignKeyConstraintConflictException);

			var expectedGroupDependencyException =
				new GroupDependencyException(invalidGroupReferenceException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(foreignKeyConstraintConflictException);

			// when
			ValueTask<Group> addGroupTask =
				this.groupService.AddGroupAsync(someGroup);

			// then
			GroupDependencyException actualGroupDependencyException = 
				 await Assert.ThrowsAsync<GroupDependencyException>(() =>
					addGroupTask.AsTask());

			actualGroupDependencyException.Should().BeEquivalentTo(
				expectedGroupDependencyException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedGroupDependencyException))),
						Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertGroupAsync(It.IsAny<Group>()),
						Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowDependencyExceptionOnCreateIfDatabaseUpdateErrorOccursAndLogItAsync()
		{
			// given
			Group someGroup = CreateRandomGroup();

			var databaseUpdateException =
				new DbUpdateException();

			var failedGroupStorageException =
				new FailedGroupStorageException(databaseUpdateException);

			var expectedGroupDependencyException =
				new GroupDependencyException(failedGroupStorageException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(databaseUpdateException);

			// when
			ValueTask<Group> addGroupTask =
				this.groupService.AddGroupAsync(someGroup);

			// then
			GroupDependencyException actualGroupDependencyException = 
				 await Assert.ThrowsAsync<GroupDependencyException>(() =>
					addGroupTask.AsTask());

			actualGroupDependencyException.Should().BeEquivalentTo(
				expectedGroupDependencyException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertGroupAsync(It.IsAny<Group>()),
					Times.Never);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedGroupDependencyException))),
						Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowServiceExceptionOnCreateIfServiceErrorOccursAndLogItAsync()
		{
			// given
			Group someGroup = CreateRandomGroup();
			var serviceException = new Exception();

			var failedGroupServiceException =
				new FailedGroupServiceException(serviceException);

			var expectedGroupServiceException =
				new GroupServiceException(failedGroupServiceException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Throws(serviceException);

			// when
			ValueTask<Group> addGroupTask =
				this.groupService.AddGroupAsync(someGroup);

			// then
			GroupServiceException actualGroupServiceException = 
				 await Assert.ThrowsAsync<GroupServiceException>(() =>
					addGroupTask.AsTask());

			actualGroupServiceException.Should().BeEquivalentTo(
				expectedGroupServiceException);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertGroupAsync(It.IsAny<Group>()),
					Times.Never);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedGroupServiceException))),
						Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}
