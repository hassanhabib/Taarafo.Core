// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            //then
            await Assert.ThrowsAsync<GroupDependencyException>(() =>
                retrieveGroupByIdTask.AsTask());

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
    }
}
