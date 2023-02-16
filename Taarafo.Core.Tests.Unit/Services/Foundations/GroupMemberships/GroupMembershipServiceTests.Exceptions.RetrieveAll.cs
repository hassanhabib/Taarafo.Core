// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.GroupMemberships.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupMemberships
{
    public partial class GroupMembershipServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedGroupMembershipStorageException =
                new FailedGroupMembershipStorageException(sqlException);

            var expectedGroupMembershipDependencyException =
                new GroupMembershipDependencyException(failedGroupMembershipStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGroupMemberships())
                    .Throws(sqlException);

            // when
            Action retrieveAllGroupMembershipsAction = () =>
                this.groupMembershipService.RetrieveAllGroupMemberships();

            GroupMembershipDependencyException actualGroupMembershipDependencyException =
                Assert.Throws<GroupMembershipDependencyException>(
                    retrieveAllGroupMembershipsAction);

            // then
            actualGroupMembershipDependencyException.Should().BeEquivalentTo(
                expectedGroupMembershipDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGroupMemberships(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupMembershipDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}