// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.Groups.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Groups
{
    public partial class GroupServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedGroupStorageException =
                new FailedGroupStorageException(sqlException);

            var expectedGroupDependencyException =
                new GroupDependencyException(failedGroupStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGroups())
                    .Throws(sqlException);

            // when
            Action retrieveAllGroupsAction = () =>
                this.groupService.RetrieveAllGroups();

            // then
            Assert.Throws<GroupDependencyException>(
                retrieveAllGroupsAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGroups(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
