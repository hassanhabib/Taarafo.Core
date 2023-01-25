// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.GroupMemberships;
using Taarafo.Core.Models.GroupMemberships.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupMemberships
{
    public partial class GroupMembershipServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            //given
            GroupMembership someGroupMembership = CreateRandomGroupMembership();
            SqlException sqlException = GetSqlException();

            var failedGroupMembershipStorageException =
                new FailedGroupMembershipStorageException(sqlException);

            var expectedGroupMembershipDependencyException =
                new GroupMembershipDependencyException(failedGroupMembershipStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            //when
            ValueTask<GroupMembership> addGroupMembershipTask =
                this.groupMembershipService.AddGroupMembershipAsync(someGroupMembership);

            GroupMembershipDependencyException actualGroupMembershipDependencyException =
                await Assert.ThrowsAsync<GroupMembershipDependencyException>(
                    addGroupMembershipTask.AsTask);

            //then
            actualGroupMembershipDependencyException.Should().BeEquivalentTo(
                expectedGroupMembershipDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupMembershipDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupMembershipAsync(It.IsAny<GroupMembership>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}