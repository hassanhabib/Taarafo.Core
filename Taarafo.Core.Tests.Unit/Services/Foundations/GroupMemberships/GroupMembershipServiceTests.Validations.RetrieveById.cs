// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.GroupMemberships;
using Taarafo.Core.Models.GroupMemberships.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupMemberships
{
    public partial class GroupMembershipServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidGroupMembershipId = Guid.Empty;

            var invalidGroupMembershipException =
                new InvalidGroupMembershipException();
            invalidGroupMembershipException.AddData(
                key: nameof(GroupMembership.Id),
                values: "Id is required");

            var expectedGroupMembershipValidationException =
                new GroupMembershipValidationException(invalidGroupMembershipException);

            // when
            ValueTask<GroupMembership> retrieveGroupMembershipByIdTask =
                this.groupMembershipService.RetrieveGroupMembershipByIdAsync(invalidGroupMembershipId);

            GroupMembershipValidationException actualGroupMembershipValidationException =
                await Assert.ThrowsAsync<GroupMembershipValidationException>(
                    retrieveGroupMembershipByIdTask.AsTask);

            // then
            actualGroupMembershipValidationException.Should().BeEquivalentTo(
                expectedGroupMembershipValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupMembershipValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupMembershipByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfGroupMembershipIsNotFoundAndLogItAsync()
        {
            //given
            Guid someGroupMembershipId = Guid.NewGuid();
            GroupMembership noGroupMembership = null;

            var notFoundGroupMembershipException =
                new NotFoundGroupMembershipException(someGroupMembershipId);

            var expectedGroupMembershipValidationException =
                new GroupMembershipValidationException(notFoundGroupMembershipException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupMembershipByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noGroupMembership);

            //when
            ValueTask<GroupMembership> retrieveGroupMembershipByIdTask =
                this.groupMembershipService.RetrieveGroupMembershipByIdAsync(someGroupMembershipId);

            GroupMembershipValidationException actualGroupMembershipValidationException =
                await Assert.ThrowsAsync<GroupMembershipValidationException>(
                    retrieveGroupMembershipByIdTask.AsTask);

            // then
            actualGroupMembershipValidationException.Should().BeEquivalentTo(
                expectedGroupMembershipValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupMembershipByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupMembershipValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}