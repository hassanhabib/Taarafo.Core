// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using Taarafo.Core.Models.Groups;
using Taarafo.Core.Models.Groups.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Groups
{
    public partial class GroupServiceTests
    {
        [Fact]
        public async Task ShouldThrowExceptionOnCreateIfGroupIsNullAndLogItAsync()
        {
            // given
            Group invalidGroup = null;

            var nullGroupException =
                new NullGroupException();

            var expectedGroupValidationException =
                new GroupValidationException(nullGroupException);

            // when
            ValueTask<Group> createGroupTask =
                this.groupService.CreateGroupAsync(invalidGroup);

            // then
            await Assert.ThrowsAsync<GroupValidationException>(() =>
                createGroupTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupAsync(invalidGroup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}