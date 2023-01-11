// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.Groups;
using Taarafo.Core.Models.Groups.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Groups
{
	public partial class GroupServiceTests
	{
		[Fact]
		public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
		{
			//given
			Guid someGroupId = Guid.NewGuid();
			SqlException sqlException = GetSqlException();

			var failedGroupStorageException =
				new FailedGroupStorageException(sqlException);

			var expectedGroupDependencyException =
				new GroupDependencyException(failedGroupStorageException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectGroupByIdAsync(It.IsAny<Guid>()))
					.ThrowsAsync(sqlException);

			//when
			ValueTask<Group> retrieveGroupByIdTask =
				this.groupService.RetrieveGroupByIdAsync(someGroupId);

			GroupDependencyException actualGroupDependencyException = 
				 await Assert.ThrowsAsync<GroupDependencyException>(
					retrieveGroupByIdTask.AsTask);

			//then
			actualGroupDependencyException.Should().BeEquivalentTo(
				expectedGroupDependencyException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectGroupByIdAsync(It.IsAny<Guid>()),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(
					expectedGroupDependencyException))),
						Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
		{
			//given
			Guid someGroupId = Guid.NewGuid();
			var serviceException = new Exception();

			var failedGroupServiceException =
				new FailedGroupServiceException(serviceException);

			var expectedGroupServiceException =
				new GroupServiceException(failedGroupServiceException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectGroupByIdAsync(It.IsAny<Guid>()))
					.ThrowsAsync(serviceException);

			//when
			ValueTask<Group> retrieveGroupByIdTask =
				this.groupService.RetrieveGroupByIdAsync(someGroupId);

		    GroupServiceException actualGroupServiceException = 
				 await Assert.ThrowsAsync<GroupServiceException>(
					 retrieveGroupByIdTask.AsTask);

			//then
			actualGroupServiceException.Should().BeEquivalentTo(
				expectedGroupServiceException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectGroupByIdAsync(It.IsAny<Guid>()),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(
					expectedGroupServiceException))),
						Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}
