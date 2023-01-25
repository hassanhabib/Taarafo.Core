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
        public async Task ShouldThrowValidationExceptionOnAddIfGroupMembershipIsNullAndLogItAsync()
        {
            // given
            GroupMembership nullGroupMembership = null;

            var nullGroupMembershipException =
                new NullGroupMembershipException();

            var expectedGroupMembershipValidationException =
                new GroupMembershipValidationException(nullGroupMembershipException);

            // when
            ValueTask<GroupMembership> addGroupMembershipTask =
                this.groupMembershipService.AddGroupMembershipAsync(nullGroupMembership);

            GroupMembershipValidationException actualGroupMembershipValidationException =
                await Assert.ThrowsAsync<GroupMembershipValidationException>(
                    addGroupMembershipTask.AsTask);

            // then
            actualGroupMembershipValidationException.Should().BeEquivalentTo(
                expectedGroupMembershipValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupMembershipValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfGroupMembershipIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidGuid = Guid.Empty;

            var invalidGroupMembership = new GroupMembership
            {
                GroupId = invalidGuid,
                ProfileId = invalidGuid
            };

            var invalidGroupMembershipException =
                new InvalidGroupMembershipException();

            invalidGroupMembershipException.AddData(
                key: nameof(GroupMembership.Id),
                values: "Id is required");

            invalidGroupMembershipException.AddData(
                key: nameof(GroupMembership.GroupId),
                values: "Id is required");

            invalidGroupMembershipException.AddData(
                key: nameof(GroupMembership.ProfileId),
                values: "Id is required");

            invalidGroupMembershipException.AddData(
                key: nameof(GroupMembership.MembershipDate),
                values: "Date is required");

            var expectedGroupMembershipValidationException =
                new GroupMembershipValidationException(invalidGroupMembershipException);

            //when
            ValueTask<GroupMembership> addGroupMembershipTask =
                this.groupMembershipService.AddGroupMembershipAsync(invalidGroupMembership);

            GroupMembershipValidationException actualGroupMembershipValidationException =
                await Assert.ThrowsAsync<GroupMembershipValidationException>(
                    addGroupMembershipTask.AsTask);

            //then
            actualGroupMembershipValidationException.Should().BeEquivalentTo(
                expectedGroupMembershipValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupMembershipValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupMembershipAsync(invalidGroupMembership),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfGroupMembershipDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTime =
                GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTime.AddMinutes(minutesBeforeOrAfter);

            GroupMembership randomGroupMembership = CreateRandomGroupMembership(invalidDateTime);
            GroupMembership invalidGroupMembership = randomGroupMembership;
            var invalidGroupMembershipException =
                new InvalidGroupMembershipException();

            invalidGroupMembershipException.AddData(
                key: nameof(GroupMembership.MembershipDate),
                values: "Date is not recent");

            var expectedGroupMembershipValidationException =
                new GroupMembershipValidationException(invalidGroupMembershipException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<GroupMembership> addGroupMembershipTask =
                this.groupMembershipService.AddGroupMembershipAsync(invalidGroupMembership);

            GroupMembershipValidationException actualGroupMembershipValidationException =
               await Assert.ThrowsAsync<GroupMembershipValidationException>(
                   addGroupMembershipTask.AsTask);

            // then
            actualGroupMembershipValidationException.Should().BeEquivalentTo(
                expectedGroupMembershipValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupMembershipValidationException))),
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