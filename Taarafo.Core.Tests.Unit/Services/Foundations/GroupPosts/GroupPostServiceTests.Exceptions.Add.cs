// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.GroupPosts.Exceptions;
using Taarafo.Core.Models.GroupPosts;
using Xunit;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Taarafo.Core.Models.PostImpressions;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupPosts
{
    public partial class GroupPostServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            GroupPost someGroupPost = CreateRandomGroupPost();
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
                this.groupPostService.AddGroupPostAsync(someGroupPost);

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

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfGroupPostAlreadyExistsAndLogItAsync()
        {
            // given
            GroupPost someGroupPost = CreateRandomGroupPost();
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsGroupPostException =
                new AlreadyExistsGroupPostException(duplicateKeyException);

            var expectedGroupPostDependencyValidationException =
                new GroupPostDependencyValidationException(alreadyExistsGroupPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGroupPostAsync(It.IsAny<GroupPost>()))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<GroupPost> addGroupPostTask =
                this.groupPostService.AddGroupPostAsync(someGroupPost);

            GroupPostDependencyValidationException actualGroupPostDependencyValidationException =
                await Assert.ThrowsAsync<GroupPostDependencyValidationException>(
                    addGroupPostTask.AsTask);

            // then
            actualGroupPostDependencyValidationException.Should().BeEquivalentTo(
                expectedGroupPostDependencyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupPostAsync(someGroupPost),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            //given
            GroupPost someGroupPost = CreateRandomGroupPost();

            var databaseUpdateException =
                new DbUpdateException();

            var failedGroupPostStorageException =
                new FailedGroupPostStorageException(databaseUpdateException);

            var expectedGroupPostDependencyException =
                new GroupPostDependencyException(failedGroupPostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGroupPostAsync(someGroupPost))
                    .Throws(databaseUpdateException);

            //when
            ValueTask<GroupPost> addGroupPostTask =
                this.groupPostService.AddGroupPostAsync(someGroupPost);

            GroupPostDependencyException actualGroupPostDependencyException =
                await Assert.ThrowsAsync<GroupPostDependencyException>(
                    addGroupPostTask.AsTask);

            //then
            actualGroupPostDependencyException.Should().BeEquivalentTo(
                expectedGroupPostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupPostAsync(It.IsAny<GroupPost>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowDependencyValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            //given
            GroupPost someGroupPost = CreateRandomGroupPost();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidGroupPostReferenceException =
                new InvalidGroupPostReferenceException(foreignKeyConstraintConflictException);

            var expectedGroupPostDependencyValidationException =
                new GroupPostDependencyValidationException(invalidGroupPostReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGroupPostAsync(someGroupPost))
                    .Throws(foreignKeyConstraintConflictException);

            //when
            ValueTask<GroupPost> addGroupPostTask =
                this.groupPostService.AddGroupPostAsync(someGroupPost);

            GroupPostDependencyValidationException actualGroupPostDependencyValidationException =
                await Assert.ThrowsAsync<GroupPostDependencyValidationException>(
                    addGroupPostTask.AsTask);

            //then
            actualGroupPostDependencyValidationException.Should().BeEquivalentTo(
                expectedGroupPostDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupPostAsync(It.IsAny<GroupPost>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
