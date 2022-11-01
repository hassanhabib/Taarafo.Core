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
using Taarafo.Core.Models.GroupPosts.Exceptions;
using Taarafo.Core.Models.GroupPosts;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupPosts
{
    public partial class GroupPostServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            GroupPost randomGroupPost = CreateRandomGroupPost(randomDateTime);
            SqlException sqlException = GetSqlException();

            var failedGroupPostStorageException =
                new FailedGroupPostStorageException(sqlException);

            var expectedGroupPostDependencyException =
                new GroupPostDependencyException(failedGroupPostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGroupPostAsync(It.IsAny<GroupPost>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<GroupPost> addGroupPostTask =
                this.groupPostService.AddGroupPostAsync(randomGroupPost);

            // then
            await Assert.ThrowsAsync<GroupPostDependencyException>(() =>
                addGroupPostTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupPostAsync(It.IsAny<GroupPost>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
